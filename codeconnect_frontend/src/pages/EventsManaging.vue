<script>
import EventManageCard from "@/components/EventManageCard.vue"
import AddEventForm from "@/components/AddEventForm.vue"
export default {
  name: "EventsManaging",
  components: {EventManageCard, AddEventForm},
  data() {
    return {
      addFormVisible: false,
      myEvents: null,
      groupsList: null,
      cityList: null,
      categoryList: null,
      tagsList: null,
      past: false,
      moderation: false,
    }
  },
  methods: {
    handleModeration(){
      if (this.moderation === true)
        this.fetchModeration();
      else{
        this.fetchMyEvents();
      }
    },
    handlePast(){
      if (this.past === true)
        this.fetchMyEventsPast();
      else{
        this.fetchMyEvents();
      }
    },
    fetchModeration(){
      this.$store.dispatch("user/getMyModerationEvents").then(
          data => {
            this.myEvents = data;
          },
          error => {
            console.log(error);
          }
      )
    },
    fetchMyEventsPast() {
      this.$store.dispatch("user/getMyOwnEventsPast").then(
          data => {
            this.myEvents = data;
          },
          error => {
            console.log(error);
          }
      )
    },
    fetchMyEvents() {
      this.$store.dispatch("user/getMyOwnEvents").then(
          data => {
            this.myEvents = data;
          },
          error => {
            console.log(error);
          }
      )
    },
    fetchTagsList() {
      this.$store.dispatch("user/getTagsList").then(
          data => {
            this.tagsList = data;
          },
          error => {
            console.log(error);
          }
      )
    },
    fetchCityList() {
      this.$store.dispatch("user/getCityList").then(
          data => {
            this.cityList = data;
          },
          error => {
            console.log(error);
          }
      )
    },
    fetchCategoryList() {
      this.$store.dispatch("user/getCategoryList").then(
          data => {
            this.categoryList = data;
          },
          error => {
            console.log(error);
          }
      )
    },
    fetchGroupsList() {
      this.$store.dispatch("user/getUserOwnGroups").then(
          data => {
            this.groupsList = data;
          },
          error => {
            console.log(error);
          }
      )
    },
    processItemRemoved(id) {
      // filter events
      this.myEvents = this.myEvents.filter(e => e.activityId !== id);
      this.$notify({
        group: "notif",
        title: "Success",
        text: "Мероприятие успешно удалено"
      }, 4000)
    },
    handleUpdate(id) {
      this.$notify({
        group: "notif",
        title: "Успешно",
        text: "Мероприятие успешно обновлено"
      }, 4000)
      this.fetchMyEvents();
    },
    requestAddForm(){
      this.addFormVisible = true;
    },
    handleCreateEvent(){
      this.addFormVisible = false;
      this.fetchMyEvents();
    }
  },
  mounted() {
    this.fetchMyEvents();
    this.fetchTagsList();
    this.fetchGroupsList();
    this.fetchCityList();
    this.fetchCategoryList();
  },
}
</script>

<template>
  <main class="mb-auto mt-12 bg-white">
    <div class="container mx-auto">
      <h1 class="text-4xl mb-6">Управление мероприятиями</h1>

      <Dialog v-model:visible="addFormVisible" modal header="Добавить мероприятие" :style="{ width: '25rem' }">
        <AddEventForm :groupsList="groupsList"
                      :cityList="cityList"
                      :categoryList="categoryList"
                      :tagsList="tagsList"
                      @createEvent="handleCreateEvent"
        />
      </Dialog>
      <div class="flex justify-between flex-row">
        <div class="flex justify-start flex-col basis-4/5">
          <EventManageCard v-for="event in myEvents"
                           :eventData="event"
                           :groupsList="groupsList"
                           :cityList="cityList"
                           :categoryList="categoryList"
                           :tagsList="tagsList"
                           @itemRemoved="processItemRemoved"
                           @updated="handleUpdate"/>
        </div>
        <div class="flex flex-col shadow basis-1/5 rounded-lg ml-4 items-center h-full pt-4 border border-slate-100 border-2">
          <div class="text-xl font-semibold text-slate-600 ">
            Меню
          </div>
          <div class="p-2 shadow bg-green-100 border-green-300 border border-solid border-2 rounded-lg w-40 mb-1 cursor-pointer hover:border-green-500 m-2 rounded-full w-36 flex justify-center items-center cursor-pointer" @click="requestAddForm">
            Добавить
          </div>
          <div class="mt-4 mb-4 flex flex-col items-center w-40">
            <label for="past">Прошедшие</label>
            <ToggleButton v-model="past" onLabel="Включено" offLabel="Выключено" @change="handlePast"/>
          </div>
          <div class="mb-4 flex flex-col items-center w-40">
            <label for="past">На модерации</label>
            <ToggleButton v-model="moderation" onLabel="Включено" offLabel="Выключено" @change="handleModeration"/>
          </div>
        </div>
      </div>

    </div>
  </main>
</template>

<style scoped>

</style>