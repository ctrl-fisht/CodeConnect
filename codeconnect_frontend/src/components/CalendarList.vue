<script>
import CalendarListDate from "@/components/CalendarListDate.vue"
import CalendarListItem from "@/components/CalendarListItem.vue"
export default {
  name: "CalendarList",
  components: {CalendarListDate, CalendarListItem},
  props: {
    monthEvents: {
      type: Map,
      required: true,
      default: null
    },
    month: {
      type: Number,
      required: true,
      default: 0
    }
  },
  methods: {
    removeFromList(eventId) {

      const idToRemove = 1;

      for (const [key, value] of this.monthEvents) {
        const updatedValue = value.filter(event => event.activityId !== eventId);
        if (updatedValue.length === 0) {
          this.monthEvents.delete(key);
        } else {
          this.monthEvents.set(key, updatedValue);
        }
      }

      console.log(this.monthEvents);
    }
  }
}
</script>

<template>
   <div class="bg-white rounded-lg py-2 px-4 ">

     <div class="flex flex-col mb-4 items-start" v-if="monthEvents.size > 0" v-for="key in monthEvents.keys()">

       <CalendarListDate :date="key" :month="month"/>
       <CalendarListItem :eventInfo="value" v-for="value in monthEvents.get(key)" @removed="removeFromList"/>
     </div>
     <div v-else class="flex flex-col mb-4 items-start text-xl"> Нет запланированных мероприятий</div>
   </div>
</template>

<style scoped>

</style>