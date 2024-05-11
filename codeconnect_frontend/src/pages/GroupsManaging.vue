<script>
import GroupCreateForm from "@/components/GroupCreateForm.vue"
import GroupManageCard from "@/components/GroupManageCard.vue"
export default {
  name: "GroupsManaging",
  components: {GroupCreateForm, GroupManageCard},
  data() {
    return {
      createFormVisible: false,
      groupsList: null,
    }
  },
  methods: {
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
    requestCreateForm() {
      this.createFormVisible = true;
    },
    handleCreateGroup() {
      this.createFormVisible = false;
      this.$notify({
        group: "notif",
        title: "Успешно",
        text: "Сообщество создано"
      }, 4000)
      this.fetchGroupsList();
    },
    processGroupRemoved(){
      this.fetchGroupsList();
    },
    handleUpdated(){
      this.fetchGroupsList();
    }
  },
  mounted() {
    this.fetchGroupsList();
  }
}
</script>

<template>
  <main class="mb-auto mt-12 bg-white">
    <div class="container mx-auto ">

      <Dialog v-model:visible="createFormVisible" modal header="Создать группу" :style="{ width: '25rem' }">
        <GroupCreateForm
            @createGroup="handleCreateGroup"
        />
      </Dialog>
      <h1 class="text-4xl mb-6">Управление группами</h1>
      <div class="flex justify-between flex-row">
        <div class="flex justify-start flex-col basis-4/5">
          <GroupManageCard v-for="group in groupsList" :groupInfo="group" @groupRemoved="processGroupRemoved" @updated="handleUpdated"/>
        </div>
        <div class="flex flex-col shadow basis-1/5 rounded-lg ml-4 items-center h-36 pt-4 border border-slate-100 border-2">
          <div class="text-xl font-semibold text-slate-600 ">
            Меню
          </div>
          <div class="p-2 shadow bg-green-100 border-green-300 border border-solid border-2 rounded-lg w-40 mb-1 cursor-pointer hover:border-green-500 m-2 rounded-full w-36 flex justify-center items-center cursor-pointer" @click="requestCreateForm">
            Добавить
          </div>
        </div>
      </div>

    </div>
  </main>
</template>

<style scoped>

</style>