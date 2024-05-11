<script>
export default {
  name: "EventModeration",
  props: {
    eventData: {
      type: Object,
      required: true
    }
  },
  methods: {
    approve(){
      this.$store.dispatch("user/approveActivity", this.eventData.activityId).then(
          data => {
            this.$emit("updated", "");
            this.$notify({
              group: "notif",
              title: "Успешно",
              text: "Мероприятие одобрено"
            }, 4000)
          },
          error => {
            this.$notify({
              group: "error",
              title: "Ошибка",
              text: error.message
            }, 4000)
          }
      );
    },
    decline() {
      this.$store.dispatch("user/declineActivity", this.eventData.activityId).then(
          data => {
            this.$emit("updated", "");
            this.$notify({
              group: "notif",
              title: "Успешно",
              text: "Мероприятие отклонено"
            }, 4000)
          },
          error => {
            this.$notify({
              group: "error",
              title: "Ошибка",
              text: error.message
            }, 4000)
          }
      );
    }
  }
}
</script>

<template>
  <div v-if="eventData">
    <div class="mt-4">
      <div class="flex flex-row items-center justify-between p-1 text-center">
        <div class="w-80 font-medium hover:font-bold">
          <router-link :to="`/events/${this.eventData.activityId}/preview`">
            {{ eventData.title }}
          </router-link>
        </div>
        <div class="w-40" >
          {{ eventData.community.name }}
        </div>
        <div class="w-40">
          {{ eventData.city.name }}
        </div>
        <div class="w-40">
          {{ eventData.dateUtc }}
        </div>
        <div class="w-40 flex flex-row items-center justify-center">
          <div class="cursor-pointer" @click="approve">
            ✅
          </div>
          <div class="cursor-pointer" @click="decline">
            ❌
          </div>
        </div>
      </div>
    </div>
  </div>

</template>

<style scoped>

</style>