<script>
import GroupHorizontalCard from "@/components/GroupHorizontalCard.vue"
import EventHorizontalCard from "@/components/EventHorizontalCard.vue"
export default {
  name: "Subscriptions",
  components: {GroupHorizontalCard, EventHorizontalCard},
  data() {
    return {
      subActivities: null,
      groupList: [],
      offset: 0,
      count: 5,
    }
  },
  methods: {
    fetchSubActivities(){
      console.log("Fetch sub activities")
      this.$store.dispatch("user/getSubscribedActivities", {offsetParam: 0, countParam: this.count}).then(
          data => {
            this.subActivities = data;
          },
          error => {
            console.log(error);
          }
      )
    },
    fetchSubscriptions() {
      this.$store.dispatch("user/getSubscriptions").then(
          data => {
            this.groupList = data;
          },
          error => {
            console.log(error);
          }
      )
    },
    handleDelete(groupId) {
      const index = this.groupList.findIndex(g => g.communityId === groupId);
      if (index !== -1) {
        this.groupList.splice(index, 1);
      }
    }
  },
  mounted() {
    this.fetchSubscriptions();
    this.fetchSubActivities();
  }
}
</script>

<template>
  <main class="mb-auto mt-12 bg-white">
    <div class="container mx-auto ">
      <div class="flex flex-row">
        <div class="basis-4/5 mr-4">
          <div>
            <h1 class="text-4xl mb-4">Мероприятия отслеживаемых групп</h1>
          </div>
          <EventHorizontalCard v-for="activity in subActivities" :eventData="activity"/>
        </div>
        <div class="flex h-full justify-start flex-col basis-1/5">
          <div v-if="groupList.length === 0" class="text-xl">Список подписок пуст</div>
          <div>
            <h1 class="text-4xl mb-4">Подписки на группы</h1>
          </div>
          <GroupHorizontalCard v-for="group in groupList" :groupInfo="group" :key="group.id" @deleted="handleDelete"/>
        </div>
      </div>

    </div>
  </main>
</template>

<style scoped>
.list-item {
  display: inline-block;
  margin-right: 10px;
}

.list-enter-active,
.list-leave-active {
  transition: all 0.1s ease;
}

.list-enter-from,
.list-leave-to {
  opacity: 0;
  transform: translateY(30px);
}

.list-move {
  transition: transform 0.5s ease;
}
</style>