<script>
export default {
  name: "EventHorizontalCard",
  props: {
    eventData: {
      type: Object,
      required: true
    }
  },
  computed: {
    getUserDateString(){
      var date = new Date(this.eventData.dateUtc);
      return date.toLocaleDateString() + ` (польз.)`;
    }
  }
}
</script>

<template>
  <router-link :to="{ path: `/events/${eventData.activityId}`}">
    <div class="flex items-center h-58 bg-white border border-slate-100 border-2 hover:bg-slate-100 shadow border rounded-xl mb-8">
      <div class="p-2 flex flex-row items-center">
        <router-link :to="{ path: `/events/${eventData.activityId}`}">
          <div class="size-48 bg-orange-200 rounded-xl flex justify-center items-center" v-if="!eventData.image">
            Нет фото
          </div>
          <img v-else :src="`${eventData.image.smallPath}`" class="size-48 rounded-xl hover:border" alt="" >
        </router-link>
        <div class="flex self-start flex-col ml-4">
          <div class="flex flex-row">
            <div class="mr-2  rounded px-1" :style="{ backgroundColor: activityTag.tag.colorHex, opacity: 0.7 }" v-for="activityTag in eventData.activityTags">
              {{ activityTag.tag.title }}
            </div>
          </div>
          <div class="font-bold text-xl ">
            {{ eventData.title }}
          </div>
          <div class="text-red-700 ">
            {{ eventData.community.name }}
          </div>
          <div class="flex flex-row items-center">
            <div class="">
              📍 {{ eventData.city.name }}
            </div>
          </div>
          <div class="rounded-xl">
            ⏰ {{ this.getUserDateString}}
          </div>
          <div class="flex flex-row mt-2" v-if="eventData.ticketPrice > 0">
            <div class="bg-red-300 rounded px-1" v-if="eventData.ticketPrice > 0">
              Платный вход
            </div>
          </div>
          <div class="flex flex-row">
            <div class="mr-4  mt-2 bg-green-100 px-1 rounded-xl" :style="{ backgroundColor: activityCategory.category.colorHex, opacity: 0.7 }" v-for="activityCategory in eventData.activityCategories">
              {{activityCategory.category.title}}
            </div>
          </div>


        </div>
      </div>
    </div>
  </router-link>


</template>

<style scoped>

</style>