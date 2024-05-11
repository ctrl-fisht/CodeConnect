<template>
  <input :value="modelValue" @input="updateInput" class="w-full py-2.5 border" type="text">
</template>



<script>
export default {
  name: 'my-input',
  props: {
    modelValue: [String, Number]
  },
  methods: {
    updateInput(event){
      this.$emit('update:modelValue', event.target.value)
    }
  }
}
</script>

<style scoped>
.input{

}
</style>