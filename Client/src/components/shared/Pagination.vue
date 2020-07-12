<template>
  <div class="container" v-if="listData && listData.length > 0">
    <slot name="heading"></slot>
    <div class="row pl-0 my-3">
        <button class="btn btn-primary btn-sm ml-3" @click="prevPage" :disabled="pageNumber === 0">Previous</button>
        <button class="btn btn-primary btn-sm ml-2" @click="nextPage" :disabled="pageNumber >= numberOfPages - 1">Next</button>
        <select v-model="itemsPerPage" name="items-per-page" id="items-per-page" class="form-control form-control-sm col-md-2 ml-md-2 ml-3 my-3 my-md-0">
            <option value="10" selected>10 items per page</option>
            <option value="50" selected>50 items per page</option>
            <option value="100" selected>100 items per page</option>
        </select>
    </div>
    <slot name="data" :paginatedData="paginatedData"></slot>
  </div>
</template>

<script>
export default {
  data: function() {
    return {
      pageNumber: 0,
      itemsPerPage: 10
    };
  },
  props: {
    listData: {
      type: Array,
      required: true
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