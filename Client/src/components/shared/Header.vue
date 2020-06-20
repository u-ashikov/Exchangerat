<template>
  <nav class="navbar navbar-expand-lg navbar-light bg-light">
    <router-link tag="a" class="navbar-brand" to="/">Exchangerat</router-link>
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

        <li class="nav-item">
            <router-link tag="a" class="nav-link" :to="{ name: 'login' }">Login</router-link>
        </li>

        <li class="nav-item">
          <router-link tag="a" class="nav-link" :to="{ name: 'register' }">Register</router-link>
        </li>
      </ul>
    </div>
  </nav>
</template>

<script>
export default {};
</script>