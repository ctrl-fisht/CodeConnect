<script>
export default {
  name: "CalendarListDate",
  props: {
    date: {
      type: String,
      required: true,
      default: ""
    },
    month: {
      type: Number,
      required: true,
      default: 0
    }
  },
  data() {
    return {
      months: new Map(
          [
            [1, "Января"],
            [2, "Февраля"],
            [3, "Марта"],
            [4, "Апреля"],
            [5, "Мая"],
            [6, "Июня"],
            [7, "Июля"],
            [8, "Августа"],
            [9, "Сентября"],
            [10, "Октября"],
            [11, "Ноября"],
            [12, "Декабря"],
          ]
      )
    }
  },
  computed: {
    dateString() {
      const monthString = this.months.get(this.month);

      let dateString = `${this.date} ${monthString}`;
      return dateString;
    }
  },
  mounted(){

  }
}
</script>

<template>
  <div class="p-1 bg-slate-100 rounded-lg font-semibold w-full">
    {{ this.dateString }}
  </div>
</template>

<style scoped>

</style>