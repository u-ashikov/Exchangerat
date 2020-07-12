<template>
    <div>
      <div v-show="requests && requests.length > 0">
        <h1 class="display-4 text-center mt-3 mb-5">My Requests</h1>
        <table class="container table table-striped">
            <thead>
                <th>Type</th>
                <th>Status</th>
                <th>Account Identity Number</th>
                <th>Issued At</th>
            </thead>
            <tbody>
                <tr v-for="request in requests" v-bind:key="request.issuedAt">
                    <td>{{ request.requestType}}</td>
                    <td>
                      <i v-bind:class="getClassByStatus(request.status)"></i>
                    </td>
                    <td>
                        <router-link tag="a" :to="{ name: 'accountTransactions', params: { accountId: request.accountId }}">{{ request.accountIdentityNumber }}</router-link>
                    </td>
                    <td>{{ request.issuedAt | transactionDate }}</td>
                </tr>
            </tbody>
        </table>
      </div>
      <div v-show="!requests || requests.length === 0 && errors.length === 0" class="container w-50 alert alert-success mx-auto my-5" role="alert" v-cloak>
        <h4 class="alert-heading">Ooooops!</h4>
        <p>Sorry, you don't have any requests yet. If you want you can create one by using the link bellow.</p>
        <hr>
        <router-link tag="a" class="btn btn-link" :to="{ name: 'createRequest' }">Create Request</router-link>
    </div>
      <div class="container alert alert-danger my-3" role="alert" v-show="errors.length !== 0" v-cloak>
          <p class="my-0" v-for="error in errors" v-bind:key="error">{{ error }}</p>
      </div>
  </div>
</template>

<script>
import ValidationError from '../../components/shared/ValidationError'

import requestGatewayService from "../../services/requestGatewayService"
import errorHandler from "../../helpers/error-handler"

import moment from 'moment'

export default {
  components: {
      validationError: ValidationError
  },
  data: function() {
    return {
      requests: [],
      errors: []
    };
  },
  filters: {
    transactionDate: function (value) {
      if (!value) { return ''; }

      return moment(value).format("MMM Do YYYY"); 
    }
  },
  mounted: function() {
    var self = this;

    requestGatewayService
      .getMy()
      .then(function(response) {
        if (response && response.data) {
            self.requests = response.data;
        }
      })
      .catch(function(error) {
        self.errors = errorHandler.extractErrorsFromResponse(error.response);
      });
  },
  methods: {
    getClassByStatus: function (status) {
      if (!status) {
        return 'fas fa-question-circle text-warning';
      }

      var className = 'fas fa-check-circle text-success';

      if (status === 'Pending') {
        className = 'far fa-circle text-warning';
      } else if (status === 'Cancelled') {
        className = 'fas fa-minus-circle text-danger';
      }

      return className;
    }
  }
};
</script>