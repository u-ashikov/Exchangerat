<template>
  <div class="col-md-3 mx-auto">
    <h1 class="text-center">Register</h1>
    <validation-error v-bind:errors="errors"></validation-error>
    <hr />
    <form method="post" v-on:submit.prevent="register">
      <div class="form-group">
        <label for="username" class="h6">Username</label>
        <input type="text" class="form-control" id="username" v-model.lazy="username" v-bind:class="{ invalid: $v.username.$error }" v-on:blur="$v.username.$touch()" />
        <p class="text-danger" v-if="$v.username.$error && !$v.username.required">The Username field is required.</p>
        <p class="text-danger" v-if="!$v.username.minLength">The Username field must be at last {{ $v.username.$params.minLength.min }} symbols length.</p>
        <p class="text-danger" v-if="!$v.username.maxLength">The Username field can be at most {{ $v.username.$params.maxLength.max }} symbols length.</p>
      </div>

      <div class="form-group">
        <label for="email" class="h6">Email</label>
        <input type="email" class="form-control" id="email" v-model.lazy="email" v-bind:class="{ invalid: $v.email.$error }" v-on:blur="$v.email.$touch()" />
        <p class="text-danger" v-if="$v.email.$error && !$v.email.required">The Email field is required.</p>
        <p class="text-danger" v-if="!$v.email.email">Please enter a valid email address.</p>
        <p class="text-danger" v-if="!$v.email.minLength">The Email address field must be at last {{ $v.email.$params.minLength.min }} symbols length.</p>
        <p class="text-danger" v-if="!$v.email.maxLength">The Email address field can be at most {{ $v.email.$params.maxLength.max }} symbols length.</p>
      </div>

      <div class="form-group">
        <label for="password" class="h6">Password</label>
        <input type="password" class="form-control" id="password" v-model.lazy="password" v-bind:class="{ invalid: $v.password.$error }" v-on:blur="$v.password.$touch()" />
        <p class="text-danger" v-if="$v.password.$error && !$v.password.required">The Password field is required.</p>
        <p class="text-danger" v-if="!$v.password.minLength">The Password field must be at last {{ $v.password.$params.minLength.min }} symbols length.</p>
        <p class="text-danger" v-if="!$v.password.maxLength">The Password field can be at most {{ $v.password.$params.maxLength.max }} symbols length.</p>
      </div>

      <div class="form-group">
        <label for="confirm-password" class="h6">Confirm Password</label>
        <input type="password" class="form-control" id="confirm-password" v-model.lazy="confirmPassword" v-bind:class="{ invalid: $v.confirmPassword.$error }" v-on:blur="$v.confirmPassword.$touch()" />
        <p class="text-danger" v-if="$v.confirmPassword.$error && !$v.confirmPassword.required">The Confirm password field is required.</p>
        <p class="text-danger" v-if="!$v.confirmPassword.sameAs">The Confirm password field must match the password field.</p>
      </div>

      <div class="form-group">
        <label for="first-name" class="h6">First Name</label>
        <input type="text" class="form-control" id="first-name" v-model.lazy="firstName" v-bind:class="{ invalid: $v.firstName.$error }" v-on:blur="$v.firstName.$touch()" />
        <p class="text-danger" v-if="$v.firstName.$error && !$v.firstName.required">The First name field is required.</p>
        <p class="text-danger" v-if="!$v.firstName.minLength">The First name field must be at last {{ $v.firstName.$params.minLength.min }} symbols length.</p>
        <p class="text-danger" v-if="!$v.firstName.maxLength">The First name field can be at most {{ $v.firstName.$params.maxLength.max }} symbols length.</p>
      </div>

      <div class="form-group">
        <label for="last-name" class="h6">Last Name</label>
        <input type="text" class="form-control" id="last-name" v-model.lazy="lastName" v-bind:class="{ invalid: $v.lastName.$error }" v-on:blur="$v.lastName.$touch()" />
        <p class="text-danger" v-if="$v.lastName.$error && !$v.lastName.required">The Last name field is required.</p>
        <p class="text-danger" v-if="!$v.lastName.minLength">The Last name field must be at last {{ $v.lastName.$params.minLength.min }} symbols length.</p>
        <p class="text-danger" v-if="!$v.lastName.maxLength">The Last name field can be at most {{ $v.lastName.$params.maxLength.max }} symbols length.</p>
      </div>

      <div class="form-group">
        <label for="address" class="h6">Address</label>
        <input type="text" class="form-control" id="address" v-model.lazy="address" v-bind:class="{ invalid: $v.address.$error }" v-on:blur="$v.address.$touch()" />
        <p class="text-danger" v-if="$v.address.$error && !$v.address.required">The Address field is required.</p>
        <p class="text-danger" v-if="!$v.address.minLength">The Address field must be at last {{ $v.address.$params.minLength.min }} symbols length.</p>
        <p class="text-danger" v-if="!$v.address.maxLength">The Address field can be at most {{ $v.address.$params.maxLength.max }} symbols length.</p>
      </div>

      <input type="submit" class="btn btn-success" value="Register" />
    </form>
    <p class="my-3">
      Already have an account? <router-link tag="a" :to="{ name: 'login' }">Sign in</router-link>
    </p>
  </div>
</template>

<script>
import ValidationError from "../shared/ValidationError"
import errorHandler from "../../helpers/error-handler"
import { validations } from '../../validations/identity/register'

import clientService from '../../services/clientService'

export default {
  components: {
    validationError: ValidationError
  },
  data: function() {
    return {
      username: "",
      email: "",
      password: "",
      confirmPassword: "",
      firstName: "",
      lastName: "",
      address: "",
      errors: []
    };
  },
  validations: validations,
  methods: {
    register: function() {
      var self = this;
      self.errors = [];

      this.$v.$touch();

      if (this.$v.$invalid) {
        return;
      }

      this.$store
        .dispatch("register", {
          username: this.username,
          email: this.email,
          password: this.password,
          confirmPassword: this.confirmPassword,
          firstName: this.firstName,
          lastName: this.lastName,
          address: this.address
        })
        .then(function(response) {
          self.errors = [];

          if (response && response.status == 200) {
            clientService.create({ firstName: self.firstName, lastName: self.lastName, address: self.address})
              .then(function (response) {
                if (response && response.status == 200) {
                  self.$store.commit('setClientData', response.data);
                  self.$store.dispatch('setLocalStorageClientData', response.data);
                }
              });
          }

          self.$router.push("/");
        })
        .catch(function(error) {
          self.errors = errorHandler.extractErrorsFromResponse(error.response);
        });
    }
  }
};
</script>