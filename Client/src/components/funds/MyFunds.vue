<template>
    <div>
      <div v-show="funds && funds.length > 0">
        <h1 class="display-4 text-center mt-3 mb-5">My Funds</h1>
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
                    <td>{{ fund.issuedAt | transactionDate }}</td>
                </tr>
            </tbody>
        </table>
      </div>
      <div class="container alert alert-danger my-3" role="alert" v-show="errors.length !== 0" v-cloak>
          <p class="my-0" v-for="error in errors" v-bind:key="error">{{ error }}</p>
      </div>
  </div>
</template>

<script>
import ValidationError from '../../components/shared/ValidationError';

import fundService from "../../services/fundService";
import errorHandler from "../../helpers/error-handler";

import moment from 'moment';
import numeral from 'numeral';

export default {
  components: {
      validationError: ValidationError
  },
  data: function() {
    return {
      funds: [],
      errors: []
    };
  },
  filters: {
    money: function (value) {
      if (!value) { return 0;}

      return numeral(value).format('0,0');
    },
    transactionDate: function (value) {
      if (!value) { return ''; }

      return moment(value).format("MMM Do YYYY"); 
    }
  },
  mounted: function() {
    var self = this;

    fundService
      .getMy()
      .then(function(response) {
        if (response && response.data) {
            console.log(response.data);
            self.funds = response.data;
        }
      })
      .catch(function(error) {
        self.errors = errorHandler.extractErrorsFromResponse(error.response);
      });
  }
};
</script>