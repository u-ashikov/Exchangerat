<template>
  <div class="col-md-4 mx-auto">
    <h1 class="text-center">Login</h1>
    <validation-error v-bind:errors="errors"></validation-error>
    <hr />
    <form method="post" v-on:submit.prevent="login">
      <div class="form-group">
        <label for="username" class="h6">Username</label>
        <input type="text" class="form-control" id="username" v-model="username" />
      </div>

      <div class="form-group">
        <label for="password" class="h6">Password</label>
        <input type="password" class="form-control" id="password" v-model="password" />
      </div>

      <input type="submit" class="btn btn-success" value="Login" />
    </form>
  </div>
</template>

<script>
import ValidationError from "../shared/ValidationError";
import errorHandler from '../../helpers/error-handler';

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
  methods: {
    login: function() {
      var self = this;
      self.errors = [];

      this.$store
        .dispatch("login", { username: this.username, password: this.password })
        .then(function(response) {
          self.errors = [];
          self.$router.push("/");
        })
        .catch(function(error) {
            self.errors = errorHandler.extractErrorsFromResponse(error.response);
        });
    }
  }
};
</script>