<script>
export default {
  name: "EventDetails",
  props: {
    eventData: {
      type: Object,
      required: true
    }
  },
  data () {
    return {
      isMember: true
    }
  },
  methods: {
    addToCalendar() {
      this.$store.dispatch("user/addToCalendar", this.eventData.activityId).then(
          data => {
            this.isMember = true;
            this.$notify({
              group: "notif",
              title: "Успешно добавлено",
              text: "Мероприятие добавлено в календарь CodeConnect"
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
    fetchIsMember() {
      this.$store.dispatch("user/isMemberOfEvent", this.eventData.activityId).then(
          data => {
            this.isMember = data.result;
          },
          error => {

          }
      );
    },
    removeEvent() {
      this.$store.dispatch("user/removeFromCalendar", this.eventData.activityId).then(
          data => {
            this.$notify({
              group: "notif",
              title: "Успешно удалено",
              text: "Мероприятие удалено из календаря CodeConnect"
            }, 4000)
            this.fetchIsMember();
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
    addToGoogleCalendar(){
      let googleLink = "https://calendar.google.com/calendar/u/0/r/eventedit";

      let cur = new Date(`${this.eventData.dateUtc}T${this.eventData.timeUtc}Z`);
      cur.setMinutes(cur.getMinutes() + this.eventData.durationMinutes);


      let datesStart = `${this.eventData.dateUtc.replaceAll("-", "")}T${this.eventData.timeUtc.replaceAll(":", "")}Z`;

      let normalizedMonth = cur.getUTCMonth() + 1;
      if (normalizedMonth < 10)
        normalizedMonth = `0${normalizedMonth}`;

      let normalizedDate = cur.getUTCDate();
      if (normalizedDate < 10)
        normalizedDate = `0${normalizedDate}`;

      let normalizedHours = cur.getUTCHours();
      if (normalizedHours < 10)
        normalizedHours = `0${normalizedHours}`;

      let normalizedMinutes = cur.getUTCMinutes();
      if (normalizedMinutes < 10)
        normalizedMinutes = `0${normalizedMinutes}`;

      let normalizedSeconds = cur.getUTCSeconds();
      if (normalizedSeconds < 10)
        normalizedSeconds = `0${normalizedSeconds}`;


      let datesEnd = `${cur.getUTCFullYear()}${normalizedMonth}${normalizedDate}T${normalizedHours}${normalizedMinutes}${normalizedSeconds}Z`;
      let datesParam = `${datesStart}/${datesEnd}`;
      googleLink += `?dates=${datesParam}`

      googleLink += `&text=${this.eventData.title}`;

      googleLink += `&details=https://localhost:8080${this.$route.fullPath}`

      googleLink += `&location=${this.eventData.city.name}, ${this.eventData.address}`

      window.open(googleLink, "_blank");
    }
  },
  computed: {
    getLocalDateString(){
      return `${this.eventData.dateLocal.replaceAll("-", ".")}`;
    },
    getLocalTimeString(){
      return `${this.eventData.timeLocal.slice(0, 5)}`
    },
    getUserDateString(){
      var date = new Date(this.eventData.dateUtc);
      return date.toLocaleDateString() + ` (польз.)`;
    }
  },
  mounted() {
    this.fetchIsMember();
    document.getElementsByClassName("github-markdown-body")[0].style.padding = "1px";
  }
}
</script>

<template>
  <div class="flex flex-col h-96 rounded-full bg-blue-100 justify-center items-center" v-if="!eventData?.image?.bannerPath">
    Организатор еще не добавил обложку
  </div>
  <div class="flex flex-col h-96 rounded-full justify-center items-center" v-else>
    <img :src="`http://localhost:5259${eventData.image.bannerPath}`" class="rounded-full h-96 w-full" alt="">
  </div>
  <div class="flex flex-row justify-between">
    <div class="basis-4/5">
      <div class="text-6xl mt-8 font-medium">
        {{ eventData.title }}
      </div>
      <div class="text-xl font-semibold text-red-400" v-if="!eventData.isActive">
        На модерации
      </div>
      <div class="flex flex-col text-xl">
        <div class="flex flex-row items-center w-1/2">
          <div class="w-8 flex justify-center">
            🇷🇺
          </div>
          <a :href="`https://ya.ru/maps?text=${eventData.city.name},${eventData.address}`" target="_blank">
            <div class="ml-2 cursor-pointer hover:font-medium">
              {{  eventData.city.name }}, {{ eventData.address }}
            </div>
          </a>

        </div>
        <div class="flex flex-row items-center w-1/2 mt-2">
          <div class="w-8 flex justify-center">
            📍
          </div>
          <div class="ml-2">
            Дата:
          </div>
          <div class="ml-2  px-1 ">
            <div class="flex flex-row font-medium">
              <div class="bg-orange-200 rounded">
                {{ getLocalDateString }}
              </div>
              <div class="ml-2">
                (г. {{eventData.city.name}})
              </div>
            </div>
          </div>
        </div>
        <div class="flex flex-row items-center w-1/2 mt-2">
          <div class="w-8 flex justify-center">
            ⏰
          </div>
          <div class="ml-2">
            Начало в:
          </div>
          <div class="flex flex-row font-medium ">
            <div class="ml-2 bg-orange-200 rounded">
              {{ getLocalTimeString }}
            </div>
            <div class="ml-2">
              (г. {{eventData.city.name}})
            </div>
          </div>
        </div>
        <div class="flex flex-row items-center w-1/2 mt-2" v-if="eventData.ticketPrice === 0">
          <div class="w-8 flex justify-center">
            💵
          </div>
          <div class="ml-2">
            Бесплатно
          </div>
        </div>
        <div class="flex flex-row items-center w-1/2 mt-2" v-else>
          <div class="w-8 flex justify-center">
            💵
          </div>
          <div class="ml-2">
            Билет: {{ eventData.ticketPrice }} Р
          </div>
        </div>
        <div class="flex flex-row items-center w-1/2 mt-2" v-if="eventData.hasStream">
          <div class="w-8 flex justify-center">
            📺
          </div>
          <div class="ml-2">
            Трансляция есть
          </div>
        </div>
        <div class="flex flex-row items-center w-1/2 mt-2" v-else>
          <div class="w-8 flex justify-center">
            🟥
          </div>
          <div class="ml-2">
            Нет трансляции
          </div>
        </div>
        <div class="flex flex-row items-center w-4/5 mt-2">
          <div class="w-8 flex justify-center">
            📄
          </div>
          <div class="ml-2">
            Категории:
          </div>
          <div class="ml-2 bg-green-100 px-1 rounded-xl" :style="{ backgroundColor: activityCategory.category.colorHex, opacity: 0.7 }" v-for="activityCategory in eventData.activityCategories">
            {{activityCategory.category.title}}
          </div>
        </div>
        <div class="flex flex-row items-center w-4/5 mt-2">
          <div class="w-8 flex justify-center">
            #️⃣
          </div>
          <div class="ml-2">
            Тэги:
          </div>
          <div class="ml-2 rounded px-1" :style="{ backgroundColor: activityTag.tag.colorHex, opacity: 0.7 }" v-for="activityTag in eventData.activityTags">
            {{ activityTag.tag.title }}
          </div>
        </div>

        <hr class="m-2 mb-8">

        <!--    flex flex-col items-center self-center w-2/5  -->
        <div class="">
          <v-md-editor class="preview" :model-value="eventData.description" mode="preview"></v-md-editor>
        </div>
      </div>
    </div>
    <div class="basis 1/5 flex flex-col items-center">
      <div class="text-xl mt-8 font-medium">
        Сообщество:

        <div class="rounded flex justify-center cursor-pointer ">
          <img :src="`http://localhost:5259${eventData?.community?.image?.miniPath}`" alt="" class="size-14 mr-2 rounded" v-if="eventData?.community?.image?.miniPath">
          <router-link class="flex items-center px-1 bg-blue-200 w-full rounded hover:bg-blue-100" :to="{path: `/groups/${eventData.community.communityId}`}">
            {{ eventData.community.name }}</router-link>
        </div>
      </div>
      <div class="rounded border border-2 border-blue-300 cursor-pointer bg-blue-200 hover:bg-blue-100 w-full text-center mt-6" v-if="eventData.websiteURL">
        <a :href="eventData.websiteURL" target="_blank"><div class="py-2 px-7">Перейти на сайт</div></a>
      </div>
      <div class="rounded border border-2 border-blue-300 cursor-pointer bg-blue-200 mt-4 hover:bg-blue-100  w-full" @click="addToGoogleCalendar">
        <div class="py-2 px-7 text-center">Добавить в Google-Календарь </div>
      </div>
      <div class="rounded border border-2 border-green-300 cursor-pointer bg-green-100 mt-4 hover:bg-green-50  w-full" @click="addToCalendar" v-if="!isMember && this.eventData.isActive">
        <div class="py-2 px-7 text-center">Добавить в календарь (CodeConnect)</div>
      </div>
      <div class="rounded border border-2 border-red-300 cursor-pointer bg-red-200 mt-4 hover:bg-red-100  w-full" @click="removeEvent" v-if="isMember && this.eventData.isActive">
        <div class="py-2 px-7 text-center">Удалить из календаря (CodeConnect)</div>
      </div>

    </div>
  </div>


</template>

<style scoped>
.github-markdown-body {
  padding-left: 1px !important;
}
</style>