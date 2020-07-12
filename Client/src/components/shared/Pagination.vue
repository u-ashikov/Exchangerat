<template>
  <div v-if="listData && listData.length > 0">
    <slot name="heading"></slot>
    <div class="container pl-0 my-3">
        <button class="btn btn-primary btn-sm" @click="prevPage" :disabled="pageNumber === 0">Previous</button>
        <button class="btn btn-primary btn-sm" @click="nextPage" :disabled="pageNumber >= numberOfPages - 1">Next</button>
    </div>
    <slot name="data" :paginatedData="paginatedData"></slot>
  </div>
</template>

<script>
export default {
  data: function() {
    return {
      pageNumber: 0
    };
  },
  props: {
    listData: {
      type: Array,
      required: true
    },
    itemsPerPage: {
      type: Number,
      required: false,
      default: 10
    }
  },
  methods: {
    nextPage: function() {
      this.pageNumber++;
    },
    prevPage: function() {
      this.pageNumber--;
    }
  },
  computed: {
    numberOfPages: function() {
      return Math.ceil(this.listData.length / this.itemsPerPage);
    },
    paginatedData: function() {
      const start = this.pageNumber * this.itemsPerPage;
      const end = start + this.itemsPerPage;

      return this.listData.slice(start, end);
    }
  }
};
</script>