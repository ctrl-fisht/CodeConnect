<script>
export default {
  name: "RemoveConfirmation",
  methods: {
    processNo(){
      this.$emit("no", "");
    },
    processYes(){
      this.$emit("yes", "");
    }
  }
}
</script>

<template>
  <div class="flex flex-col justify-center items-center text-xl">
    <div>
      Вы уверены что хотите удалить объект?
    </div>
    <div class="flex flex-row justify-between w-64 mt-12">
      <div class="bg-green-300 py-2 px-8 rounded cursor-pointer hover:bg-green-200" @click="processYes">
        Да
      </div>
      <div class="bg-red-300 py-2 px-8 rounded cursor-pointer hover:bg-red-200" @click="processNo">
        Нет
      </div>
    </div>
  </div>

</template>

<style scoped>

</style>