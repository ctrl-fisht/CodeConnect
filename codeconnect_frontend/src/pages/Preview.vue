<script>
import EventInfo from "@/components/EventInfo.vue"
export default {
  name: "Preview",
  components: {EventInfo},
  data() {
    return {
      eventData: null
    }
  },
  methods: {
    fetchEventPreview() {
      this.$store.dispatch("user/getEventPreview", this.$route.params.id).then(
          data => {
            this.eventData = data;
          },
          error => {
            console.log(error);
          }
      );
    }
  },
  mounted() {
    this.fetchEventPreview();
  }
}


</script>

<template>
  <main class="mb-auto mt-12 bg-white">
    <div class="container mx-auto">
      <EventInfo v-if="eventData" :eventData="eventData"/>
    </div>
  </main>
</template>

<style scoped>

</style>