<script>
export default {
  name: "GroupInfo",
  data() {
    return {
      isSubscriber: false
    }
  },
  props: {
    groupData: {
      type: Object,
      required: true
    }
  },
  methods: {
    fetchIsSubscriber() {
      this.$store.dispatch("user/isSubscriber", this.groupData.communityId).then(
          data => {
            this.isSubscriber = data.result;
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
    handleSubscribe() {
      this.$store.dispatch("user/subscribe", this.groupData.communityId).then(
          data => {
            this.isSubscriber = true;
            this.$notify({
              group: "notif",
              title: "Успешно",
              text: "Вы подписались"
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
    handleUnsubscribe(){
      this.$store.dispatch("user/unsubscribe", this.groupData.communityId).then(
          data => {
            this.isSubscriber = false;
            this.$notify({
              group: "notif",
              title: "Успешно",
              text: "Вы отписались"
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
  },
  mounted() {
    this.fetchIsSubscriber();
  }
}
</script>

<template>
  <div class="flex flex-col">
    <div class="flex flex-row items-center justify-between">
      <div class="flex flex-row items-center">
        <div class="h-48 w-48 rounded-lg bg-blue-100 flex justify-center items-center" v-if="!groupData.image">
          Нет фото
        </div>
        <div class="h-48 w-48 rounded-lg bg-blue-100 flex justify-center items-center" v-else>
          <img :src="`http://localhost:5259${groupData.image.bigPath}`" class="rounded-lg" alt="">
        </div>
        <div class="text-4xl font-semibold ml-4 self-start">
          {{ groupData.name}}
        </div>
      </div>
      <div class="rounded cursor-pointer bg-green-300" v-if="!isSubscriber">
        <div class="py-2 px-7" @click="handleSubscribe">Подписаться</div>
      </div>

      <div class="rounded cursor-pointer bg-red-300" v-if="isSubscriber">
        <div class="py-2 px-7" @click="handleUnsubscribe">Отписаться</div>
      </div>
    </div>
    <div class="mt-4 text-xl flex flex-row">
      <div  class="font-semibold">
        Описание:
      </div>
      <div class="ml-2">
        {{ groupData.description }}
      </div>
    </div>
    <div class="mt-4 text-xl flex flex-row">
      <div  class="font-semibold">
        Email:
      </div>
      <div class="ml-2" v-if="groupData?.email">
        {{ groupData.email }}
      </div >
      <div class="ml-2" v-else>
        Не указан
      </div>
    </div>
    <div class="mt-4 text-xl flex flex-row">
      <div class="font-semibold">
        Telegram:
      </div>
      <div class="ml-2" v-if="groupData?.telegramTag">
        <a :href="`https://t.me/${groupData.telegramTag}`" target="_blank">@{{ groupData.telegramTag }}</a>
      </div >
      <div class="ml-2" v-else>
        Не указан
      </div>
    </div>
    <div class="mt-4 text-xl flex flex-row">
      <div  class="font-semibold">
        Телефон:
      </div>
      <div class="ml-2" v-if="groupData?.phone">
        {{ groupData.phone }}
      </div >
      <div class="ml-2" v-else>
        Не указан
      </div>
    </div>
  </div>
</template>

<style scoped>

</style>