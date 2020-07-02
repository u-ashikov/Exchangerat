<template>
<div class="row">
    <div class="col-4">
      <img class="col-12" width="40" height="50" src="../../assets/logo.png" />
      <p>
        Send money easily.
      </p>
    </div>
    <div class="col-md-4 mx-auto">
    <h1 class="text-center">Login</h1>
    <validation-error v-bind:errors="errors"></validation-error>
    <hr />
    <form method="post" v-on:submit.prevent="login">
      <div class="form-group">
        <label for="username" class="h6">Username</label>
        <input type="text" class="form-control" id="username" v-model.lazy="username" v-bind:class="{ invalid: $v.username.$error }" v-on:blur="$v.username.$touch()" />
        <p class="text-danger" v-if="$v.username.$error && !$v.username.required">The Username is required.</p>
      </div>

      <div class="form-group">
        <label for="password" class="h6">Password</label>
        <input type="password" class="form-control" id="password" v-model.lazy="password" v-bind:class="{ invalid: $v.password.$error }" v-on:blur="$v.password.$touch()" />
        <p class="text-danger" v-if="$v.password.$error && !$v.password.required">The Password field is required.</p>
      </div>

      <input type="submit" class="btn btn-success" value="Login" />
    </form>
  </div>
</div>
</template>

<script>
import ValidationError from "../shared/ValidationError"
import errorHandler from '../../helpers/error-handler'
import { validations } from '../../validations/identity/login'
import clients from '../../queries/clients.js'

export default {
  components: {
    validationError: ValidationError
  },
  data: function() {
    return {
      username: "",
      password: "",
      errors: []
    };
  },
  validations: validations,
  methods: {
    login: function() {
      var self = this;
      self.errors = [];

      this.$v.$touch();

      if (this.$v.$invalid) {
        return;
      }

      this.$store
        .dispatch("login", { username: this.username, password: this.password })
        .then(function(response) {
          self.errors = [];

          if (response && response.status == 200) {
              clients.getIdByUserId()
              .then(function (response) {
                  if (response && response.status == 200) {
                    self.$store.commit('setClientData', response.data);
                    self.$store.dispatch('setLocalStorageClientData', response.data);
                  }
              })
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