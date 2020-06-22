<template>
    <ul>
        <li class="text-danger" v-for="error in errors" v-bind:key="error">{{error}}</li>
    </ul>
</template>

<script>
export default {
    props: {
        errors: Array
    }
}
</script>