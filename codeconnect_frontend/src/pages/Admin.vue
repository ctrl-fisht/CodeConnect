<script>
import EventModeration from "@/components/EventModeration.vue"
export default {
  components: {EventModeration},
  name: "Admin",
  data() {
    return {
      eventList: null
    }
  },
  methods: {
    fetchAdminEvents(){
      this.$store.dispatch("user/getAdminActivities").then(
          data => {
            this.eventList = data;
          },
          error => {
            this.$router.push('/');
          }
      );
    }
  },
  mounted() {
    this.fetchAdminEvents();
  }
}
</script>

<template>
  <main class="mb-auto mt-12 bg-white">
    <div class="container mx-auto">
      <div class="text-2xl">
        Модерация
      </div>
      <div v-if="eventList" class="shadow mt-6 p-4 rounded-xl border border-2 border-slate-100">
        <div class="flex flex-row items-center justify-between font-semibold shadow rounded-lg p-1 text-center">
          <div class="w-80">
            Название
          </div>
          <div class="w-40">
            Сообщество
          </div>
          <div class="w-40">
            Город
          </div>
          <div class="w-40">
            Дата
          </div>
          <div class="w-40">
            Действие
          </div>
        </div>
        <EventModeration v-for="event in eventList" v-if="eventList" :eventData="event" @updated="fetchAdminEvents()"/>
      </div>
    </div>
  </main>
</template>

<style scoped>

</style>