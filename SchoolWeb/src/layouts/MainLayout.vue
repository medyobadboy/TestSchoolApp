<template>
  <q-layout view="hHh lpR fFf">

    <q-header elevated class="bg-primary text-white">
      <q-toolbar>
        <q-btn dense flat round icon="menu" @click="toggleLeftDrawer" />

        <q-toolbar-title>
          <q-avatar>
            <q-icon name="school" size="lg" />
          </q-avatar>
          School
        </q-toolbar-title>
      </q-toolbar>
    </q-header>

    <q-drawer show-if-above v-model="leftDrawerOpen" side="left" bordered>
      <!-- drawer content -->
      <q-scroll-area class="fit">
        <q-list>
          <template v-for="(menuItem, index) in menuList" :key="index">
            <q-item clickable v-ripple :to="menuItem.route">
              <!-- <router-link :to="menuItem.route">
                
              </router-link> -->
              <q-item-section avatar>
                <q-icon :name="menuItem.icon" />
              </q-item-section>
              <q-item-section>
                {{ menuItem.label }}
              </q-item-section>

            </q-item>
            <q-separator :key="'sep' + index" v-if="menuItem.separator" />
          </template>
        </q-list>
      </q-scroll-area>
    </q-drawer>

    <q-page-container>
      <router-view />
    </q-page-container>
  </q-layout>
</template>

<script>
import { ref } from 'vue'

const menuList =
  [
    {
      icon: 'people',
      label: 'Students',
      separator: true,
      route: '/students'
    },
    {
      icon: 'view_list',
      label: 'Courses',
      separator: true,
      route: '/courses'
    },
    {
      icon: 'perm_identity',
      label: 'Application',
      separator: true,
      route: '/applications'
    },
  ]

export default {
  setup() {
    const leftDrawerOpen = ref(false)

    return {
      leftDrawerOpen,
      menuList,
      toggleLeftDrawer() {
        leftDrawerOpen.value = !leftDrawerOpen.value
      }
    }
  }
}
</script>