<script>
import GroupInfo from "@/components/GroupInfo.vue"
import EventHorizontalCard from "@/components/EventHorizontalCard.vue"
export default {
  components: {GroupInfo, EventHorizontalCard},
  name: "GroupDetails",
  data() {
    return {
      groupInfo: null,
      groupEvents: []
    }
  },
  methods: {
    fetchDetails() {
      this.$store.dispatch("user/getGroupDetails", this.$route.params.id).then(
          data => {
            this.groupInfo = data;
          },
          error => {
            console.log(error);
          }
      );
    },
    fetchGroupEvents() {
      this.$store.dispatch("user/getGroupEvents", this.$route.params.id).then(
          data => {
            this.groupEvents = data;
          },
          error => {
            console.log(error);
          }
      );
    }
  },
  mounted() {
    this.fetchDetails();
    this.fetchGroupEvents();
  }
}
</script>

<template>
  <main class="mb-auto mt-12 bg-white">
    <div class="container mx-auto">
      <div>
        <GroupInfo v-if="groupInfo" :groupData="groupInfo"/>
      </div>
      <div  v-if="this.groupEvents.length > 0">
        <div class="text-xl mt-8 mb-2">
          Мероприятия этого сообщества
        </div>
        <div>
          <EventHorizontalCard v-for="event in groupEvents" v-if="groupEvents" :eventData="event"/>
        </div>
      </div>
      <div class="mt-12" v-else>У этого сообщества пока нет мероприятий</div>


    </div>
  </main>
</template>

<style scoped>

</style>