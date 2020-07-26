<template>
  <div>
    <div v-show="accountInfo">
      <h1 class="display-4 text-center mt-3 mb-5">Account Transactions</h1>
      <div class="container row mx-auto my-3">
        <div class="col-3 row">
          <p class="col-12 font-weight-bold my-0">Account:</p>
          <p class="col-12">
            <i class="fas fa-money-check"></i>
            {{ accountInfo.identityNumber }}
          </p>
        </div>
        <div class="col-3 row">
          <p class="col-12 font-weight-bold my-0">Balance</p>
          <p class="col-12">
            <i class="fas fa-balance-scale"></i>
            {{ accountInfo.balance | money }}
          </p>
        </div>
        <div class="col-3 row">
          <p class="col-12 font-weight-bold my-0">Created At</p>
          <p class="col-12">
            <i class="fas fa-calendar-week"></i>
            {{ accountInfo.createdAt | transactionDate }}
          </p>
        </div>
      </div>

      <form v-show="transactions.length !== 0" class="container form-inline my-4">
        <div class="form-group">
            <label for="start-date" class="h6">Start Date</label>
            <input type="date" id="start-date" class="form-control form-control-sm mx-sm-3" v-model="search.startDate">
        </div>
        <div class="form-group">
            <label for="end-date" class="h6">End Date</label>
            <input type="date" id="end-date" class="form-control form-control-sm mx-sm-3" v-model="search.endDate">
        </div>
        <div class="form-group">
            <label for="transactionType" class="h6">Type</label>
            <select name="transactionType" id="transactionType" v-model="search.transactionType" class="form-control form-control-sm mx-sm-3">
              <option value="in">In</option>
              <option value="out">Out</option>
            </select>
        </div>
        <input type="button" class="btn btn-primary btn-sm" value="Reset" v-on:click="resetForm" />
    </form>

      <pagination v-bind:list-data="filteredItems">
        <template #data="{ paginatedData }">
          <div v-show="paginatedData && paginatedData.length > 0">
            <table class="container table table-striped">
              <thead>
                <th>Sender Account</th>
                <th>Receiver Account</th>
                <th>Type</th>
                <th>Amount</th>
                <th>Description</th>
                <th>Issued At</th>
              </thead>
              <tbody>
                <tr v-for="(transaction, index) in paginatedData" v-bind:key="index">
                  <td>{{ transaction.senderAccountNumber}}</td>
                  <td>{{ transaction.receiverAccountNumber}}</td>
                  <td v-if="transaction.senderAccountNumber === accountInfo.identityNumber">
                    <i class="fas fa-arrow-alt-circle-down text-danger"></i>
                  </td>
                  <td v-else-if="transaction.receiverAccountNumber === accountInfo.identityNumber">
                    <i class="fas fa-arrow-alt-circle-up text-success"></i>
                  </td>
                  <td>&dollar; {{ transaction.amount | money }}</td>
                  <td>{{ transaction.description}}</td>
                  <td>{{ transaction.issuedAt | transactionDate }}</td>
                </tr>
              </tbody>
            </table>
          </div>
        </template>
        <template #create-button>
          <router-link tag="a" class="btn btn-primary btn-sm ml-sm-3 ml-2" :to="{ name: 'createTransaction' }">Create</router-link>
        </template>
      </pagination>
    </div>
    <div
      class="container alert alert-danger my-3"
      role="alert"
      v-show="errors.length !== 0"
      v-cloak
    >
      <p class="my-0" v-for="error in errors" v-bind:key="error">{{ error }}</p>
    </div>

    <div v-if="transactions.length !== 0 && (!filteredItems || filteredItems.length == 0)" class="container alert alert-warning" role="alert">
      No transactions found!
    </div>

    <div
      v-show="!accountInfo || transactions.length === 0 && errors.length === 0"
      class="container w-50 alert alert-success mx-auto my-5"
      role="alert"
      v-cloak
    >
      <h4 class="alert-heading">Ooooops!</h4>
      <p>Sorry, you don't have any transactions yet. If you want you can create one by using the link bellow.</p>
      <hr />
      <router-link tag="a" class="btn btn-link" :to="{ name: 'createTransaction' }">Create Transaction</router-link>
    </div>

  </div>
</template>

<script>
import ValidationError from "../../components/shared/ValidationError"
import Pagination from "../../components/shared/Pagination"

import exchangeAccountService from "../../services/exchangeAccountService"
import errorHandler from "../../helpers/error-handler"

import moment from "moment"
import numeral from "numeral"

import flatpickr from 'flatpickr'
import "flatpickr/dist/flatpickr.css"

export default {
  components: {
    validationError: ValidationError,
    pagination: Pagination
  },
  data: function() {
    return {
      accountInfo: {},
      transactions: [],
      errors: [],
      search: {
        startDate: moment().startOf('month').format('YYYY-MM-DD'),
        endDate: moment().endOf('month').format('YYYY-MM-DD'),
        transactionType: ''
      }
    };
  },
  methods: {
    resetForm: function () {
      this.search.startDate = moment().startOf('month').format('YYYY-MM-DD');
      this.search.endDate = moment().endOf('month').format('YYYY-MM-DD');
      this.search.transactionType = '';
    }
  },
  filters: {
    money: function(value) {
      if (!value) {
        return 0;
      }

      return numeral(value).format("0,0");
    },
    transactionDate: function(value) {
      if (!value) {
        return "";
      }

      return moment(value).format("MMM Do YYYY");
    }
  },
  mounted: function() {
    flatpickr("#start-date");
    flatpickr("#end-date");

    var self = this;

    exchangeAccountService
      .getAccountDetailsById(this.$route.params.accountId)
      .then(function(response) {
        if (response && response.data) {
          self.accountInfo = {
            accountType: response.data.accountType,
            balance: response.data.balance,
            identityNumber: response.data.identityNumber,
            createdAt: response.data.createdAt
          };

          self.transactions = response.data.transactions;
        }
      })
      .catch(function(error) {
        self.errors = errorHandler.extractErrorsFromResponse(error.response);
      });
  },
  computed: {
    filteredItems: function () {
      var self = this;

      var filteredTransactions = self.transactions;

      if (!self.search.startDate && !self.search.endDate) {
        return filteredTransactions;
      }

      if (self.search.transactionType === 'in') {
        filteredTransactions = filteredTransactions.filter(function (transaction) {
          return transaction.receiverAccountNumber === self.accountInfo.identityNumber;
        });
      }

      if (self.search.transactionType === 'out') {
        filteredTransactions = filteredTransactions.filter(function (transaction) {
          return transaction.senderAccountNumber === self.accountInfo.identityNumber;
        });
      }

      return filteredTransactions.filter(function (transaction) {
        if (self.search.startDate && !self.search.endDate) {
          return transaction.issuedAt >= self.search.startDate;
        }

        if (self.search.endDate && !self.search.startDate) {
          return transaction.issuedAt <= self.search.endDate;
        }

        return transaction.issuedAt >= self.search.startDate && transaction.issuedAt <= self.search.endDate;
      });
    }
  }
};
</script>