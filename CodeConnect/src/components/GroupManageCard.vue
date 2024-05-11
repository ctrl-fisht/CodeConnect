<script>
import RemoveConfirmation from "@/components/RemoveConfirmation.vue"
import EditGroupForm from "@/components/EditGroupForm.vue"
import EditCommunityPhoto from "@/components/EditCommunityPhoto.vue"
export default {
  name: "GroupManageCard",
  components: {RemoveConfirmation, EditGroupForm, EditCommunityPhoto},
  data() {
    return {
      removeConfirmationVisible: false,
      editFormVisible: false,
      editPhotoFormVisible: false,
    }
  },
  props: {
    groupInfo: {
      type: Object,
      required: true
    }
  },
  methods: {
    requestRemoveConfirmation() {
      this.removeConfirmationVisible = true;
    },
    removeCancel() {
      this.removeConfirmationVisible = false;
    },
    removeProcess() {
      this.removeDialogVisible = false;
      this.$store.dispatch("user/removeGroup", this.groupInfo.communityId).then(
          data => {
            this.$notify({
              group: "notif",
              title: "Успешно",
              text: "Сообщество удалено"
            }, 4000)
            this.$emit("groupRemoved", this.groupInfo.communityId);
          },
          error => {
            this.$notify({
              group: "error",
              title: "Ошибка",
              text: "Не удалось удалить сообщество"
            }, 4000)
            console.log(error);
          }
      )
    },
    requestGroupEdit() {
      this.editFormVisible = true;
    },
    handleUpdated() {
      this.editFormVisible = false;
      this.$notify({
        group: "notif",
        title: "Успешно",
        text: "Сообщество обновлено"
      }, 4000)
      this.$emit("updated", "");
    },
    handleUpdatedPhoto(){
      this.editPhotoFormVisible = false;
      this.$emit("updated", "");
      this.$notify({
        group: "notif",
        title: "Успешно",
        text: "Фото обновлено"
      }, 4000)
      location.reload();
    },
    requestEditPhoto(){
      this.editPhotoFormVisible = true;
    }
  }
}
</script>

<template>

  <Dialog v-model:visible="editFormVisible" modal header="Изменить группу" :style="{ width: '25rem' }">
    <EditGroupForm :groupInfo="groupInfo" @updateEvent="handleUpdated"/>
  </Dialog>
  <Dialog v-model:visible="removeConfirmationVisible" modal header="Удалить группу" :style="{ width: '25rem' }">
    <RemoveConfirmation @no="removeCancel" @yes="removeProcess"/>
  </Dialog>
  <Dialog v-model:visible="editPhotoFormVisible" modal header="Изменить фото" :style="{ width: '25rem' }">
    <EditCommunityPhoto :communityId="groupInfo.communityId" @updateCommunity="handleUpdatedPhoto"/>
  </Dialog>

  <div class="">
    <div class="flex items-center border border-slate-100 border-2 flex-row h-58 bg-white hover:border-slate-200 hover:bg-slate-100 shadow border rounded-xl mb-8 justify-between">
      <div class="p-4 flex flex-row items-center basis-5/6">
        <router-link :to="{ path: `/groups/${groupInfo.communityId}`}">
          <div class="size-48 bg-red-200 rounded-xl flex justify-center items-center"  v-if="!groupInfo.image">
            Без фото
          </div>
          <div class="size-48 rounded-xl bg-blue-200 flex justify-center items-center" v-else>
            <img :src="`http://localhost:5259${groupInfo.image.smallPath}?r`" class="rounded-xl" alt="">
          </div>
        </router-link>
        <div class="flex flex-col self-start ml-4 mt-4">
          <router-link :to="{ path: `/groups/${groupInfo.communityId}`}">
            <div class="text-xl font-semibold cursor-pointer hover:text-amber-300">
              {{groupInfo.name}}
            </div>
          </router-link>
          <div class="flex flex-row items-center mt-4">
            <div class="font-semibold">
              Описание:
            </div>
            <div class="ml-2">
              {{groupInfo.description}}
            </div>
          </div>
          <div class="mt-1 flex flex-row">
            <div  class="font-semibold">
              Email:
            </div>
            <div class="ml-2" v-if="groupInfo?.email">
              {{ groupInfo.email }}
            </div >
            <div class="ml-2" v-else>
              Не указан
            </div>
          </div>
          <div class="mt-1 flex flex-row">
            <div class="font-semibold">
              Telegram:
            </div>
            <div class="ml-2" v-if="groupInfo?.telegramTag">
              <a :href="`https://t.me/${groupInfo.telegramTag}`" target="_blank">@{{ groupInfo.telegramTag }}</a>
            </div >
            <div class="ml-2" v-else>
              Не указан
            </div>
          </div>
          <div class="mt-1 flex flex-row">
            <div  class="font-semibold">
              Телефон:
            </div>
            <div class="ml-2" v-if="groupInfo?.phone">
              {{ groupInfo.phone }}
            </div >
            <div class="ml-2" v-else>
              Не указан
            </div>
          </div>
        </div>

      </div>
      <div class="flex flex-col p-4 items-center basis-1/6">
        <div class="flex justify-center p-1 shadow bg-blue-100 border-blue-300 border border-solid border-2 rounded-lg w-44 mb-1 cursor-pointer hover:border-blue-500" @click="requestGroupEdit">
          Редактировать
        </div>
        <div class="flex justify-center p-1 shadow bg-blue-100 border-blue-300 border border-solid border-2 rounded-lg w-44 mb-1 cursor-pointer hover:border-blue-500" @click="requestEditPhoto">
          Изменить фото
        </div>
        <div class="flex justify-center p-1 shadow bg-red-100 border-red-300 border border-solid border-2 rounded-lg w-44 mb-1 cursor-pointer hover:border-red-500" @click="requestRemoveConfirmation">
          Удалить
        </div>
      </div>
    </div>
  </div>


</template>

<style scoped>

</style>