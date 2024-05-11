<template>
  <div class="dialog" v-if="show === true" @click.stop="hide" >
    <div @click.stop class="dialog__content bg-orange-50 border-2 border-slate-200 rounded-2xl ">
      <slot></slot>
    </div>
  </div>
</template>

<script>
import toggleMixin from "@/mixins/toggleMixin";

export default {
  name: 'my-dialog',
  mixins: [toggleMixin]
}
</script>

<style scoped>
.dialog {
  top: 0;
  bottom: 0;
  right: 0;
  left: 0;
  background: rgba(0,0,0, 0.5);
  position: fixed;
  display: flex;
}

.dialog__content {
  margin: auto;
  min-height: 200px;
  min-width: 500px;
  padding: 20px;
}
</style>