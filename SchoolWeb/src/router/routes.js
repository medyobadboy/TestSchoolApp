const routes = [
  {
    path: '/',
    component: () => import('layouts/MainLayout.vue'),
    children: [
      { path: '', component: () => import('pages/IndexPage.vue') },
      { path: 'students', component: () => import('pages/StudentsPage.vue') },
      { path: 'students/:id', component: () => import('pages/StudentPage.vue') },
      { path: 'new_student', component: () => import('pages/NewStudentPage.vue') },
      { path: 'courses', component: () => import('pages/CoursesPage.vue') },
      { path: 'courses/:id', component: () => import('pages/CoursePage.vue') },
      { path: 'new_course', component: () => import('pages/NewCoursePage.vue') },
      { path: 'applications', component: () => import('pages/ApplicationsPage.vue') },
      { path: 'applications/:id', component: () => import('pages/ApplicationPage.vue') },
      { path: 'new_application', component: () => import('pages/NewApplicationPage.vue') }
    ]
  },

  // Always leave this as last one,
  // but you can also remove it
  {
    path: '/:catchAll(.*)*',
    component: () => import('pages/ErrorNotFound.vue')
  }
]

export default routes
