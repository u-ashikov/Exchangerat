<template>
    <div class="col-md-6 mx-auto">
    <h1 class="text-center">Login</h1>
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
export default {
    data: function () {
        return {
            username: '',
            password: ''
        }
    },
    methods: {
        login: function () {
            this.$store.dispatch('login', { username: this.username, password: this.password });

            this.username = '';
            this.password = '';

            this.$router.push('/');
        }
    }
}
</script>