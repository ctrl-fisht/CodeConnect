<script>
import GroupHorizontalCard from "@/components/GroupHorizontalCard.vue"
export default {
  name: "Subscriptions",
  components: {GroupHorizontalCard},
  data() {
    return {
      telegramConnected: false,
      notifEnabled: false,
      tag: ""
    }
  },
  methods: {
    openTgBot(uid){
      let url = `https://t.me/develop_azaza_bot?start=${uid}`
      window.open(url);
    },
    connectTg() {
      if (this.tag === "")
        return;

      this.$store.dispatch("user/startTgConnect", this.tag).then(
          data => {
            this.$notify({
              group: "notif",
              title: "Успешно",
              text: "Начат процесс привязки"
            }, 4000)
            this.openTgBot(data.uid);
          },
          error => {
            this.$notify({
              group: "error",
              title: "Ошибка",
              text: error.message
            }, 4000)
          }
      )
    },
    removeTg(){
      if (!this.telegramConnected)
        return;

      this.$store.dispatch("user/removeTg", this.tag).then(
          data => {
            this.$notify({
              group: "notif",
              title: "Успешно",
              text: "Телеграм успешно отвязан"
            }, 4000)
            this.telegramConnected = false;
          },
          error => {
            this.$notify({
              group: "error",
              title: "Ошибка",
              text: error.message
            }, 4000)
          }
      )
    },
    fetchTgStatus(){
      this.$store.dispatch("user/getTgStatus").then(
          data => {
            this.telegramConnected = data.connected;
          },
          error => {
            console.log(error);
          }
      )
    },
    fetchTgNotifStatus(){
      this.$store.dispatch("user/getNotifStatus").then(
          data => {
            this.notifEnabled = data.enabled;

          },
          error => {
            console.log("fetchTgNotifStatusError")
            console.log(error);
          }
      )
    },
    enableNotif(){
      this.$store.dispatch("user/enableNotif").then(
          data => {
            this.notifEnabled = true;
            this.$notify({
              group: "notif",
              title: "Успешно",
              text: "Уведомления включены"
            }, 4000)
          },
          error => {
            this.$notify({
              group: "error",
              title: "Ошибка",
              text: error.message
            }, 4000)
          }
      )
    },
    disableNotif() {
      this.$store.dispatch("user/disableNotif").then(
          data => {
            this.notifEnabled = false;
            this.$notify({
              group: "notif",
              title: "Успешно",
              text: "Уведомления выключены"
            }, 4000)
          },
          error => {
            this.$notify({
              group: "error",
              title: "Ошибка",
              text: error.message
            }, 4000)
          }
      )
    }
  },
  mounted() {
    this.fetchTgStatus();
    this.fetchTgNotifStatus();
  }

}
</script>

<template>
  <main class="mb-auto mt-12 bg-white">
    <div class="container mx-auto ">
      <h1 class="text-4xl">Настройки</h1>
      <div class="flex justify-start flex-col">
        <div class="flex  mt-12">
          <div class="self-center text-3xl font-semibold">
            Телеграм
          </div>
          <div class="self-center ml-2">
            <img src="@/static/tg.webp" class="h-8 w-8" alt="">
          </div>
        </div>
        <div v-if="!telegramConnected" class="mt-2">
          Тэг:
        </div>
        <div v-if="!telegramConnected" class=" flex justify-start items-center">
          <div>
            <input type="text" placeholder="@tag" v-model:="tag" name="tag" class="rounded-xl px-2 h-8 w-36">
          </div>
          <div class="ml-4 bg-blue-200 p-1 rounded-xl hover:bg-blue-100 cursor-pointer" @click="connectTg">
            Привязать
          </div>
        </div>

        <div class="mt-2 text-lg flex items-center" v-else>
          <div>
            Вы привязали телеграм
          </div>
          <div class="ml-4 bg-red-400 p-1 rounded hover:bg-red-300 cursor-pointer" @click="removeTg">
            Отвязать
          </div>
        </div>

        <div class="mt-6">
          <div class="text-3xl font-semibold">
            Настройки оповещений
          </div>
          <div class="text-xl mt-2">
            Уведомления TG
          </div>
          <div v-if="!notifEnabled">
            <div class="mt-2 bg-green-400 p-1 w-24 flex justify-center rounded cursor-pointer hover:bg-green-300" v-if="!notifEnabled" @click="enableNotif">
              Включить
            </div>
          </div>
          <div v-else>
            <div class="mt-2 bg-red-400 p-1 w-24 flex justify-center rounded cursor-pointer hover:bg-red-300" @click="disableNotif">
              Выключить
            </div>
          </div>


        </div>

      </div>
    </div>
  </main>
</template>

<style scoped>

</style>