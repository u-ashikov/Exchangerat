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
      <pagination v-bind:list-data="transactions">
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
                <tr v-for="transaction in paginatedData" v-bind:key="transaction.issuedAt">
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
import ValidationError from "../../components/shared/ValidationError";
import Pagination from "../../components/shared/Pagination";

import exchangeAccountService from "../../services/exchangeAccountService";
import errorHandler from "../../helpers/error-handler";

import moment from "moment";
import numeral from "numeral";

export default {
  components: {
    validationError: ValidationError,
    pagination: Pagination
  },
  data: function() {
    return {
      accountInfo: {},
      transactions: [],
      errors: []
    };
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
  }
};
</script>