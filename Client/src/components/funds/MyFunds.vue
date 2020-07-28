<template>
    <div>
      <h1 class="display-4 text-center mt-3 mb-5">My Funds</h1>
      <form v-show="funds.length !== 0" class="container form-inline my-4">
        <div class="form-group">
            <label for="start-date" class="h6">Start Date</label>
            <input type="date" id="start-date" class="form-control form-control-sm mx-sm-3" v-model="search.startDate">
        </div>
        <div class="form-group">
            <label for="end-date" class="h6">End Date</label>
            <input type="date" id="end-date" class="form-control form-control-sm mx-sm-3" v-model="search.endDate">
        </div>
        <input type="button" class="btn btn-primary btn-sm" value="Reset" v-on:click="resetForm" />
      </form>

      <pagination v-bind:list-data="filteredItems">
          <template #data="{ paginatedData }">
              <div v-show="paginatedData && paginatedData.length > 0">
                <table class="container table table-striped">
                    <thead>
                        <th>Card Identity Number</th>
                        <th>Receiver Account</th>
                        <th>Amount</th>
                        <th>Issued At</th>
                    </thead>
                    <tbody>
                        <tr v-for="fund in funds" v-bind:key="fund.issuedAt">
                            <td>{{ fund.cardIdentityNumber}}</td>
                            <td>
                                <router-link tag="a" :to="{ name: 'accountTransactions', params: { accountId: fund.accountId }}">{{ fund.accountIdentityNumber }}</router-link>
                            </td>
                            <td>&dollar; {{ fund.amount | money }}</td>
                            <td>{{ fund.issuedAt | date }}</td>
                        </tr>
                    </tbody>
                </table>
              </div>
          </template>
          <template #create-button>
            <router-link tag="a" class="btn btn-primary btn-sm ml-sm-3 ml-2" :to="{ name: 'add-funds' }">Create</router-link>
          </template>
      </pagination>
      
      <div v-show="!funds || funds.length === 0 && errors.length === 0" class="container w-50 alert alert-success mx-auto my-5" role="alert" v-cloak>
        <h4 class="alert-heading">Ooooops!</h4>
        <p>Sorry, you don't have any funds yet. If you want you can add some by using the link bellow.</p>
        <hr>
        <router-link tag="a" class="btn btn-link" :to="{ name: 'add-funds' }">Add Funds</router-link>
      </div>

      <div class="container alert alert-warning" v-if="funds.length !== 0 && (!filteredItems || filteredItems.length == 0)" role="alert">
        No funds found!
      </div>

      <div class="container alert alert-danger my-3" role="alert" v-show="errors.length !== 0" v-cloak>
          <p class="my-0" v-for="error in errors" v-bind:key="error">{{ error }}</p>
      </div>
  </div>
</template>

<script>
import ValidationError from '../../components/shared/ValidationError'
import Pagination from '../../components/shared/Pagination'

import fundService from "../../services/fundService"
import errorHandler from "../../helpers/error-handler"

import money from '../../filters/money'
import date from '../../filters/date'

import moment from 'moment'

import flatpickr from 'flatpickr'
import "flatpickr/dist/flatpickr.css"

export default {
  components: {
      validationError: ValidationError,
      pagination: Pagination
  },
  data: function() {
    return {
      funds: [],
      errors: [],
      search: {
        startDate: moment().startOf('month').format('YYYY-MM-DD'),
        endDate: moment().endOf('month').format('YYYY-MM-DD')
      }
    };
  },
  filters: {
    money,
    date
  },
  mounted: function() {
    flatpickr("#start-date");
    flatpickr("#end-date");

    var self = this;

    fundService
      .getMy()
      .then(function(response) {
        if (response && response.data) {
            self.funds = response.data;
        }
      })
      .catch(function(error) {
        self.errors = errorHandler.extractErrorsFromResponse(error.response);
      });
  },
  methods: {
    resetForm: function () {
      this.search.startDate = moment().startOf('month').format('YYYY-MM-DD');
      this.search.endDate = moment().endOf('month').format('YYYY-MM-DD');
    }
  },
  computed: {
    filteredItems: function () {
      var self = this;

      if (!self.search.startDate && !self.search.endDate) {
        return self.funds;
      }

      return self.funds.filter(function (fund) {
        if (self.search.startDate && !self.search.endDate) {
          return fund.issuedAt >= self.search.startDate;
        }

        if (self.search.endDate && !self.search.startDate) {
          return fund.issuedAt <= self.search.endDate;
        }

        return fund.issuedAt >= self.search.startDate && fund.issuedAt <= self.search.endDate;
      });
    }
  }
};
</script>