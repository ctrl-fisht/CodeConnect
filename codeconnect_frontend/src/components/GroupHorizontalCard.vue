<script>
import axios from 'axios'
export default {
  name: "GroupHorizontalCard",
  props: {
    groupInfo: {
      type: Object,
      default: null
    }
  },
  methods: {
    async unsubscribe() {
      this.$store.dispatch("user/unsubscribe", this.groupInfo.communityId).then(
          data => {
            this.$emit("deleted", this.groupInfo.communityId);

            this.$notify({
              group: "notif",
              title: "Успешно",
              text: "Вы отписались"
            }, 4000)

          },
          (error) => {
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
    <div class="mb-4 w-full bg-white rounded-xl bg-white shadow border border-2 border-slate-200 hover:bg-slate-100">
      <div class="p-6 flex flex-row justify-around">
        <div class=" flex flex-row basis-3/5">
          <router-link :to="{path: `/groups/${groupInfo.communityId}`}">
            <div class="size-16 rounded-xl bg-blue-200 flex justify-center items-center" v-if="!groupInfo.image">
              Картинка
            </div>
            <div class="size-16 rounded-xl bg-blue-200 flex justify-center items-center" v-else>
              <img :src="`${groupInfo.image.smallPath}`" class="rounded-xl" alt="">
            </div>
          </router-link>
          <div class="ml-4 flex flex-col">
            <router-link :to="{path: `/groups/${groupInfo.communityId}`}">
            <div class="text-2xl font-semibold hover:text-orange-300 w-64">{{ groupInfo.name }}</div>
            </router-link>
          </div>
        </div>
        <div class="ml-4 flex items-start">
          <div @click="unsubscribe" class="bg-red-300 p-1 rounded-xl hover:bg-red-200 cursor-pointer">Отписаться</div>
        </div>
      </div>
    </div>
</template>

<style scoped>
</style>