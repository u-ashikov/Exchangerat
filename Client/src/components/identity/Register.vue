<template>
  <div class="col-md-6 mx-auto">
    <h1 class="text-center">Register</h1>
    <hr />
    <form method="post" v-on:submit.prevent="register">
      <div class="form-group">
        <label for="username" class="h6">Username</label>
        <input type="text" class="form-control" id="username" v-model="username" />
      </div>

      <div class="form-group">
        <label for="email" class="h6">Email</label>
        <input type="email" class="form-control" id="email" v-model="email" />
      </div>

      <div class="form-group">
        <label for="password" class="h6">Password</label>
        <input type="password" class="form-control" id="password" v-model="password" />
      </div>

      <div class="form-group">
        <label for="confirm-password" class="h6">Confirm Password</label>
        <input type="password" class="form-control" id="confirm-password" v-model="confirmPassword" />
      </div>

      <div class="form-group">
        <label for="first-name" class="h6">First Name</label>
        <input type="text" class="form-control" id="first-name" v-model="firstName" />
      </div>

      <div class="form-group">
        <label for="last-name" class="h6">Last Name</label>
        <input type="text" class="form-control" id="last-name" v-model="lastName" />
      </div>

      <div class="form-group">
        <label for="address" class="h6">Address</label>
        <input type="text" class="form-control" id="address" v-model="address" />
      </div>

      <input type="submit" class="btn btn-success" value="Register" />
    </form>
  </div>
</template>

<script>
export default {
  data: function() {
    return {
      username: "",
      email: "",
      password: "",
      confirmPassword: "",
      firstName: "",
      lastName: "",
      address: ""
    };
  },
  methods: {
    register: function() {
      this.$store.dispatch("register", {
        username: this.username,
        email: this.email,
        password: this.password,
        confirmPassword: this.confirmPassword,
        firstName: this.firstName,
        lastName: this.lastName,
        address: this.address
      })
      .then(function (response) {
        console.log(response);
      })
      .catch(function (error) {
        console.log('There is an error', error);
      });
    }
  }
};
</script>