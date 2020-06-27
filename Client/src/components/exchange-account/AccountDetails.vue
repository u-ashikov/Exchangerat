<template>
    <div>
      <validation-error v-bind:errors="errors" v-cloak></validation-error>
      <div v-show="accountInfo && accountInfo.transactions && accountInfo.transactions.length > 0">
        <h1 class="display-4 text-center mt-3 mb-5">Account Transactions</h1>
        <div class="container row mx-auto my-3">
            <div class="col-3 row">
                <p class="col-12 font-weight-bold my-0">Account:</p>
                <p class="col-12"><i class="fas fa-money-check"></i> {{ accountInfo.accountNumber }}</p>
            </div>
            <div class="col-3 row">
              <p class="col-12 font-weight-bold my-0">Balance</p>
              <p class="col-12"><i class="fas fa-balance-scale"></i> {{ accountInfo.balance | money }}</p>
            </div>
            <div class="col-3 row">
              <p class="col-12 font-weight-bold my-0">Created At</p>
              <p class="col-12"><i class="fas fa-calendar-week"></i> {{ accountInfo.createdAt | transactionDate }}</p>
            </div>
        </div>
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
                <tr v-for="transaction in accountInfo.transactions" v-bind:key="transaction.senderAccountNumber">
                    <td>{{ transaction.senderAccountNumber}}</td>
                    <td>{{ transaction.receiverAccountNumber}}</td>
                    <td v-if="transaction.senderAccountNumber === accountInfo.accountNumber"><i class="fas fa-arrow-alt-circle-down text-danger"></i></td>
                    <td v-else-if="transaction.receiverAccountNumber === accountInfo.accountNumber"><i class="fas fa-arrow-alt-circle-up text-success"></i></td>
                    <td>&dollar; {{ transaction.amount | money }}</td>
                    <td>{{ transaction.description}}</td>
                    <td>{{ transaction.issuedAt | transactionDate }}</td>
                </tr>
            </tbody>
        </table>
      </div>
      <div class="container alert alert-danger" role="alert" v-show="errors.length !== 0" v-cloak>
          Sorry, something went wrong!
      </div>
  </div>
</template>

<script>
import ValidationError from '../../components/shared/ValidationError';

import exchangeAccounts from "../../queries/exchangeAccounts";
import { errorHandler } from '../../helpers/error-handler';

import moment from 'moment';
import numeral from 'numeral';

export default {
  components: {
      validationError: ValidationError
  },
  data: function() {
    return {
      accountInfo: {},
      errors: []
    };
  },
  filters: {
    money: function (value) {
      if (!value) { return '';}

      return numeral(value).format('0,0');
    },
    transactionDate: function (value) {
      if (!value) { return ''; }

      return moment(value).format("MMM Do YYYY"); 
    }
  },
  mounted: function() {
    var self = this;

    exchangeAccounts
      .getDetailsById(this.$route.params.accountId)
      .then(function(response) {
        if (response && response.data) {
            self.accountInfo = response.data;
        }
      })
      .catch(function(error) {
        self.errors = errorHandler.extractErrorsFromResponse(error.response);
      });
  }
};
</script>