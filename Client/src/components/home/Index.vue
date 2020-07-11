<template>
  <div class="jumbotron container my-5">
    <img class="d-block align-top mx-auto" alt="" loading="lazy" src="../../assets/logo.png"/>
    <h6 class="text-center">Send money easily</h6>
    <p
      class="lead my-5"
    >This is a simple money exchange platform, which allows you to send money easily between your own accounts or send money to other people with accounts in the system.</p>
    <hr class="my-4" />
    <p class="my-0">
        Don't have an account? <router-link tag="a" :to="{ name: 'register' }">Sign up</router-link> or <router-link tag="a" :to="{ name: 'login' }">Sign in</router-link> if you already have one.
    </p>
  </div>
</template>

<script>
export default {
    
}
</script>