<template>
  <nav class="navbar navbar-expand-lg navbar-light bg-light">
    <router-link tag="a" class="navbar-brand" to="/">
      <img width="45" height="30" class="d-inline-block align-top" alt="" loading="lazy" src="../../assets/only-logo.png"/>
      Exchangerat
    </router-link>
    <button
      class="navbar-toggler"
      type="button"
      data-toggle="collapse"
      data-target="#navbarNav"
      aria-controls="navbarNav"
      aria-expanded="false"
      aria-label="Toggle navigation"
    >
      <span class="navbar-toggler-icon"></span>
    </button>
    <div class="collapse navbar-collapse" id="navbarNav">
      <ul class="navbar-nav">
        <li class="nav-item active">
          <router-link tag="a" class="nav-link" to="/">Home</router-link>
        </li>

        <li class="nav-item" v-if="!isAuthenticated">
          <router-link tag="a" class="nav-link" :to="{ name: 'login' }">Login</router-link>
        </li>

        <li class="nav-item" v-if="!isAuthenticated">
          <router-link tag="a" class="nav-link" :to="{ name: 'register' }">Register</router-link>
        </li>

        <li class="nav-item" v-if="isAuthenticated">
            <router-link tag="a" class="nav-link" :to="{ name: 'myAccounts' }">My Accounts</router-link>
        </li>

        <li class="nav-item" v-if="isAuthenticated">
          <a v-if="isAuthenticated" href="/" class="nav-item nav-link ml-auto" v-on:click="logout">Logout</a>
        </li>
      </ul>
    </div>
  </nav>
</template>

<script>
export default {
  computed: {
    isAuthenticated: function() {
      return this.$store.getters.isAuthenticated;
    }
  },
  methods: {
    logout: function() {
      this.$store.dispatch('logout');
    }
  }
};
</script>