<template>
  <div v-if="!loading ">
    <h1 class="display-4 text-center mt-3 mb-5">My Requests</h1>

    <form v-show="requests.length !== 0" class="container form-inline my-4">
        <div class="form-group">
            <label for="status" class="h6">Status</label>
            <select name="status" id="status" v-model="search.status" class="form-control form-control-sm mx-sm-3">
              <option value="Pending">Pending</option>
              <option value="Approved">Approved</option>
              <option value="Cancelled">Cancelled</option>
            </select>
        </div>
        <div class="form-group">
            <label for="type" class="h6">Type</label>
            <select name="type" id="type" v-model="search.type" class="form-control form-control-sm mx-sm-3">
              <option v-for="(requestType, index) in requestTypes" v-bind:key="index" v-bind:value="requestType.id">{{ requestType.name}}</option>
            </select>
        </div>
        <input type="button" class="btn btn-primary btn-sm" value="Reset" v-on:click="resetForm" />
    </form>

    <pagination v-bind:list-data="filteredItems">
      <template #data="{ paginatedData }">
        <div v-show="paginatedData && paginatedData.length > 0">
          <table class="container table table-striped">
            <thead>
              <th>Type</th>
              <th>Status</th>
              <th>Account Identity Number</th>
              <th>Issued At</th>
            </thead>
            <tbody>
              <tr v-for="(request, index) in paginatedData" v-bind:key="index">
                <td>{{ request.requestType}}</td>
                <td>
                    <i v-bind:class="getClassByStatus(request.status)"></i>
                </td>
                <td>
                  <router-link
                    tag="a"
                    :to="{ name: 'accountTransactions', params: { accountId: request.accountId }}"
                  >{{ request.accountIdentityNumber }}</router-link>
                </td>
                <td>{{ request.issuedAt | transactionDate }}</td>
              </tr>
            </tbody>
          </table>
        </div>
      </template>
      <template #create-button>
        <router-link tag="a" class="btn btn-primary btn-sm ml-sm-3 ml-2" :to="{ name: 'createRequest' }">Create</router-link>
      </template>
    </pagination>

    <div
      v-show="!requests || (requests.length === 0 && errors.length === 0)"
      class="container w-50 alert alert-success mx-auto my-5"
      role="alert"
      v-cloak
    >
      <h4 class="alert-heading">Ooooops!</h4>
      <p>Sorry, you don't have any requests yet. If you want you can create one by using the link bellow.</p>
      <hr />
      <router-link tag="a" class="btn btn-link" :to="{ name: 'createRequest' }">Create Request</router-link>
    </div>

    <div v-if="requests.length !== 0 && (!filteredItems || filteredItems.length == 0)" class="container alert alert-warning" role="alert">
      No requests found!
    </div>

    <div
      class="container alert alert-danger my-3"
      role="alert"
      v-show="errors.length !== 0"
      v-cloak
    >
      <p class="my-0" v-for="error in errors" v-bind:key="error">{{ error }}</p>
    </div>
  </div>
</template>

<script>
import ValidationError from "../../components/shared/ValidationError"
import Pagination from "../../components/shared/Pagination"

import requestGatewayService from "../../services/requestGatewayService"
import requestTypeService from '../../services/requestTypeService'
import errorHandler from "../../helpers/error-handler"

import moment from "moment"

export default {
  components: {
    validationError: ValidationError,
    pagination: Pagination
  },
  data: function() {
    return {
      requests: [],
      requestTypes: [],
      errors: [],
      loading: true,
      search: {
        status: '',
        type: ''
      }
    };
  },
  filters: {
    transactionDate: function(value) {
      if (!value) {
        return "";
      }

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
          self.loading = false;
        }
      })
      .catch(function(error) {
        self.loading = false;
        self.errors = errorHandler.extractErrorsFromResponse(error.response);
      });

      requestTypeService
        .getAll()
        .then(function ( response) {
            if (response && response.status === 200 && response.data) {
                self.requestTypes = response.data.data;
            }
        })
        .catch(function (error) {
            var errors = errorHandler.extractErrorsFromResponse(error.response);
            self.errors.push(errors);
        });
  },
  methods: {
    getClassByStatus: function(status) {
      if (!status) {
        return "fas fa-question-circle text-warning";
      }

      var className = "fas fa-check-circle text-success";

      if (status === "Pending") {
        className = "far fa-circle text-warning";
      } else if (status === "Cancelled") {
        className = "fas fa-minus-circle text-danger";
      }

      return className;
    },
    resetForm: function () {
      this.search.status = '';
      this.search.type = '';
    }
  },
  computed: {
    filteredItems: function () {
      var self = this;

      var filteredRequests = self.requests;

      if (self.search.status) {
        filteredRequests = filteredRequests.filter(function (request) {
          return request.status === self.search.status;
        });
      }

      if (self.search.type) {
        filteredRequests = filteredRequests.filter(function (request) {
          var requestType = self.requestTypes.filter(function (rt) {
            return rt.id === self.search.type;
          })[0];

          if (!requestType) {
            return request;
          }

          return request.requestType === requestType.name;
        });
      }

      return filteredRequests;
    }
  }
};
</script>