<script>
export default {
  name: "CalendarListItem",
  props: {
    eventInfo: {
      type: Object,
      required: true
    }
  },
  methods: {
    removeEvent() {
      this.$store.dispatch("user/removeFromCalendar", this.eventInfo.activityId).then(
          data => {
            this.$notify({
              group: "notif",
              title: "Успешно",
              text: "Мероприятие удалено"
            }, 4000)
            this.$emit("removed", this.eventInfo.activityId);
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
  <div class="flex flex-row text-xl justify-between w-full  items-center">
    <div class="flex flex-row">
      <div class="w-36">
        {{ eventInfo.timeUser}}
      </div>
      <div>
        <router-link :to="{path: `/events/${this.eventInfo.activityId}`}" class="hover:text-orange-400">
          {{ eventInfo.title }}
        </router-link>
      </div>
    </div>
    <div class="bg-red-400 px-2 flex justify-center items-center rounded-lg text-sm cursor-pointer hover:bg-red-200"
         @click="removeEvent">
      <div >
        x
      </div>

    </div>
  </div>
</template>

<style scoped>

</style>