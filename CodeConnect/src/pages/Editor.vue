<script>
export default {
  name: "Editor",
  components: {},
  data() {
    return {
      haveAccess: true,
      text: "",
      eventData: null
    }
  },
  methods: {
    isHaveAccess(){
      this.$store.dispatch("user/getIsMyActivity", this.$route.params.id).then(
          data => {
            if (!data.result){
              this.$router.push("/events/my")
            }
          },
          error => {
            console.log(error);
          }
      )
    },
    fetchEventData(){
      this.$store.dispatch("user/getEventPreview", this.$route.params.id).then(
          data => {
            this.eventData = data;
            this.text = this.eventData.description;
          },
          error => {
            console.log(error);
          }
      );
    },
    saved(){
      let payload = {
        "activityId": this.eventData.activityId,
        "description": this.text
      };
      this.$store.dispatch("user/updateEvent", payload).then(
          data => {
            this.$notify({
              group: "notif",
              title: "Успешно",
              text: "Изменения сохранены"
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
  mounted(){
    this.isHaveAccess();
    this.fetchEventData();
  },

}
</script>

<template>
  <main class="mb-auto mt-12 bg-white">
    <div class="container mx-auto">
      <div class="text-4xl">
        Редактор MD
      </div>
      <div class="mt-2 bg-blue-200 w-20 text-center p-1 rounded hover:bg-blue-100">
        <Button @click="this.$router.push('/events/my')">Назад </Button>
      </div>
      <div class="mt-4">
        <v-md-editor v-model="text" height="800px" @save="saved"></v-md-editor>
      </div>
    </div>
  </main>
</template>

<style scoped>

</style>