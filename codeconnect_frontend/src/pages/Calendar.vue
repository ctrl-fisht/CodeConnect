<script>
import CalendarList from "@/components/CalendarList.vue"
export default {
  name: "Calendar",
  components: {CalendarList},
  data() {
    return {
      monthEvents: null,
      year: 2024,
      month: 0,
      months: new Map(
          [
            [1, "Январь"],
            [2, "Февраль"],
            [3, "Март"],
            [4, "Апрель"],
            [5, "Май"],
            [6, "Июнь"],
            [7, "Июль"],
            [8, "Август"],
            [9, "Сентябрь"],
            [10, "Октябрь"],
            [11, "Ноябрь"],
            [12, "Декабрь"],
          ]
      ),
      past: false
    }
  },
  methods: {
    handlePast() {
      if (this.past === false)
      {
        this.fetchMonthEvents();
        return;
      }


      this.$store.dispatch("user/getMyEventsByMonthPast", {month: this.month, year: this.year}).then(
          data => {

            const myDate = new Date();
            const userUtcOffset = -myDate.getTimezoneOffset() / 60;
            console.log(data);
            const eventsMap = new Map();

            data.forEach(event => {
              let userDate = new Date(`${event.dateUtc}T${event.timeUtc}Z`);
              console.log("user date")
              console.log(userDate);
              let hours = "";
              let minutes = "";
              if (userDate.getHours() < 10)
                hours = `0${userDate.getHours()}`
              else
                hours = `${userDate.getHours()}`

              if (userDate.getMinutes() < 10)
                minutes = `0${userDate.getMinutes()}`;
              else
                minutes = `${userDate.getMinutes()}`;

              event.timeUser = `${hours}:${minutes}`;

              const eventDay = userDate.getDate();

              if (eventsMap.has(eventDay)) {
                eventsMap.get(eventDay).push(event);
              } else {
                eventsMap.set(eventDay, [event]);
              }
            });
            this.monthEvents = eventsMap;
          },
          error => {
            console.log(error);
          }
      );
    },
    fetchMonthEvents(){
      this.$store.dispatch("user/getMyEventsByMonth", {month: this.month, year: this.year}).then(
          data => {

            const myDate = new Date();
            const userUtcOffset = -myDate.getTimezoneOffset() / 60;
            console.log(data);
            const eventsMap = new Map();

            data.forEach(event => {
              let userDate = new Date(`${event.dateUtc}T${event.timeUtc}Z`);
              console.log("user date")
              console.log(userDate);
              let hours = "";
              let minutes = "";
              if (userDate.getHours() < 10)
                hours = `0${userDate.getHours()}`
              else
                hours = `${userDate.getHours()}`

              if (userDate.getMinutes() < 10)
                minutes = `0${userDate.getMinutes()}`;
              else
                minutes = `${userDate.getMinutes()}`;

              event.timeUser = `${hours}:${minutes}`;

              const eventDay = userDate.getDate();

              if (eventsMap.has(eventDay)) {
                eventsMap.get(eventDay).push(event);
              } else {
                eventsMap.set(eventDay, [event]);
              }
            });
            this.monthEvents = eventsMap;
          },
          error => {
            console.log(error);
          }
      );
    },
    initDate(){
      var now = new Date();
      this.month = now.getMonth() + 1;
      this.year = now.getFullYear();
    },
    monthDec() {
      if (this.month === 1){
        this.month = 12;
        this.year -= 1;
      }
      else {
        this.month -= 1;
      }
      this.fetchMonthEvents();
    },
    monthInc() {
      if (this.month === 12){
        this.month = 1;
        this.year += 1;
      }
      else {
        this.month += 1;
      }
      this.fetchMonthEvents();
    }

  },
  mounted() {
    this.initDate();
    this.fetchMonthEvents();
  }
}
</script>

<template>
  <main class="mb-auto mt-12 bg-white">
    <div class="container mx-auto">
      <div>
        <h1 class="text-4xl">Мой календарь</h1>
      </div>
      <div class="flex flex-row justify-between mt-2 mb-4">
        <div class=" flex flex-col items-center w-40">
          <label for="past">Прошедшие</label>
          <ToggleButton v-model="past" onLabel="Включено" offLabel="Выключено" @change="handlePast"/>
        </div>
        <div class="text-2xl flex flex-row">
          <div class="bg-orange-200 px-1 rounded-full cursor-pointer hover:bg-orange-100 h-8" @click="monthDec"> < </div>
          <div class="w-48 flex justify-center">
            <div>
              {{ months.get(month) }}, {{ year }}
            </div>
          </div>
          <div class="bg-orange-200 px-1 rounded-full cursor-pointer hover:bg-orange-100 h-8" @click="monthInc"> > </div>
        </div>
        <div class="bg-orange-300 p-1 rounded-lg cursor-pointer hover:bg-orange-200 h-8">
          Список
        </div>
      </div>
      <CalendarList v-if="monthEvents" :monthEvents="monthEvents" :month="month" class="shadow"/>
    </div>
  </main>
</template>

<style scoped>

</style>