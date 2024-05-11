<script>
import RemoveConfirmation from "@/components/RemoveConfirmation.vue"
import EditEventForm from "@/components/EditEventForm.vue"
import EditEventSmallPhotoForm from "@/components/EditEventSmallPhotoForm.vue"
import EditEventBannerForm from "@/components/EditEventBannerForm.vue"
export default {
  name: "EventManageCard",
  components: {RemoveConfirmation, EditEventForm, EditEventSmallPhotoForm, EditEventBannerForm},
  props: {
    eventData: {
      type: Object,
      required: true
    },
    groupsList: {
      type: Object,
      required: true
    },
    cityList: {
      type: Object,
      required: true
    },
    categoryList: {
      type: Object,
      required: true
    },
    tagsList: {
      type: Object,
      required: true
    }
  },
  data() {
    return {
      removeDialogVisible: false,
      editDialogVisible: false,
      editPhotoDialogVisible: false,
      editBannerDialogVisible: false
    }
  },
  methods: {
    requestEditDialog() {
      this.editDialogVisible = true;
    },
    requestEditBannerDialog() {
      this.editBannerDialogVisible = true;
    },
    hideEditDialog() {
      this.editDialogVisible = false;
    },
    requestRemoveDialog() {
      this.removeDialogVisible = true;
    },
    requestEditPhotoDialog(){
      this.editPhotoDialogVisible = true;
    },
    removeCancel() {
      this.removeDialogVisible = false;
    },
    removeProcess() {
      this.removeDialogVisible = false;
      this.$store.dispatch("user/removeEvent", this.eventData.activityId).then(
          data => {
            this.$emit("itemRemoved", this.eventData.activityId);
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
    handleUpdated(id) {
      this.hideEditDialog();
      this.$emit("updated", "");
    },
    handleUpdateSmallPhoto(){
      this.editPhotoDialogVisible = false;
      this.$emit("updated", "");
      location.reload();
    },
    handleUpdateBanner(){
      this.editBannerDialogVisible = false;
      this.$emit("updated");
      location.reload();

    }
  }
}
</script>

<template>
  <div>
    <Dialog v-model:visible="removeDialogVisible" modal header="Удаление мероприятие" :style="{ width: '25rem' }">
      <RemoveConfirmation @no="removeCancel" @yes="removeProcess"/>
    </Dialog>

    <Dialog v-model:visible="editDialogVisible" modal header="Редактирование мероприятие" :style="{ width: '25rem' }">
      <EditEventForm
          :eventData="this.eventData"
          :groupsList="groupsList"
          :cityList="cityList"
          :categoryList="categoryList"
          :tagsList="tagsList"
          @updateEvent="handleUpdated"
      />
    </Dialog>

    <Dialog v-model:visible="editPhotoDialogVisible" modal header="Изменить фото" :style="{ width: '25rem' }">
      <EditEventSmallPhotoForm :activityId="eventData.activityId" @updateEvent="handleUpdateSmallPhoto"/>
    </Dialog>

    <Dialog v-model:visible="editBannerDialogVisible" modal header="Изменить баннер" :style="{ width: '25rem' }">
      <EditEventBannerForm :activityId="eventData.activityId" @updateEvent="handleUpdateBanner"/>
    </Dialog>

    <div class="flex items-center border border-slate-100 border-2 flex-row h-58 bg-white hover:border-slate-200 hover:bg-slate-50 shadow border rounded-xl mb-8 justify-between">
      <div class="p-2 flex flex-row items-center basis-5/6">
        <router-link :to="{ path: `/events/${eventData.activityId}/preview`}">
          <div class="size-48 bg-red-200 rounded-xl flex justify-center items-center" v-if="!eventData?.image?.smallPath">
            Здесь фото
          </div>
          <div class="size-48 rounded-xl flex justify-center items-center" v-else>
            <img :src="`http://localhost:5259${eventData.image.smallPath}`" class="rounded-xl" alt="">
          </div>
        </router-link>
        <div class="flex self-start flex-col ml-4">
          <div class="flex flex-row">
            <div class="mr-2  rounded px-1" :style="{ backgroundColor: activityTag.tag.colorHex, opacity: 0.7 }" v-for="activityTag in eventData.activityTags">
              {{ activityTag.tag.title }}
            </div>
          </div>
          <div class="font-bold hover:text-orange-400">
            <router-link :to="{ path: `/events/${eventData.activityId}/preview`}">{{ eventData.title }}</router-link>
          </div>
          <div class="text-red-700 text-sm font-semibold hover:text-amber-200">
            <router-link :to="{ path: `/groups/${eventData.community.communityId}`}" >{{ eventData.community.name }}</router-link>
          </div>
          <div class="flex flex-row items-center">
            <div class="mr-2">
              <img src="@/static/location.svg" alt="">
            </div>
            <div class="font-medium">
              {{ eventData.city.name }}
            </div>
            <div class="ml-4 bg-amber-100 rounded-xl px-1">
              {{ eventData.dateLocal }}
            </div>
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
      <div class="flex flex-col p-4 items-center basis-1/6">
        <div class="flex justify-center p-1 shadow bg-blue-100 border-blue-300 border border-solid border-2 rounded-lg w-44 mb-1 cursor-pointer hover:border-blue-500" @click="requestEditPhotoDialog">
          Изменить фото
        </div>
        <div class="flex justify-center p-1 shadow bg-blue-100 border-blue-300 border border-solid border-2 rounded-lg w-44 mb-1 cursor-pointer hover:border-blue-500" @click="requestEditBannerDialog">
          Изменить баннер
        </div>
        <div class="flex justify-center p-1 shadow bg-blue-100 border-blue-300 border border-solid border-2 rounded-lg w-44 mb-1 cursor-pointer hover:border-blue-500" @click="requestEditDialog">
          Редактировать
        </div>
        <router-link :to="`/events/${eventData.activityId}/editor`">
          <div class="flex justify-center p-1 shadow bg-blue-100 border-blue-300 border border-solid border-2 rounded-lg w-44 mb-1 cursor-pointer hover:border-blue-500">
            Ред. описание
          </div>
        </router-link>

        <div class="flex justify-center p-1 shadow bg-red-100 border-red-300 border border-solid border-2 rounded-lg w-44 mb-1 cursor-pointer hover:border-red-500" @click="requestRemoveDialog">
          Удалить
        </div>
      </div>
    </div>
  </div>

</template>

<style scoped>

</style>