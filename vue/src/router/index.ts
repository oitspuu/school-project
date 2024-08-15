import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import LoginView from '@/views/LoginView.vue'
import { useAuthStore } from '@/stores/auth'
import CoursesView from '@/views/courses/CoursesView.vue'
import HobbiesView from '@/views/hobbies/HobbiesView.vue'
import SleepView from '@/views/sleeps/SleepView.vue'
import WorkView from '@/views/work/WorkView.vue'
import CourseDetails from '@/views/courses/CourseDetails.vue'
import CourseDelete from '@/views/courses/CourseDelete.vue'
import CourseEdit from '@/views/courses/CourseEdit.vue'
import HobbyAdd from '@/views/hobbies/HobbyAdd.vue'
import HobbyDelete from '@/views/hobbies/HobbyDelete.vue'
import HobbyEdit from '@/views/hobbies/HobbyEdit.vue'
import SleepDelete from '@/views/sleeps/SleepDelete.vue'
import WorkAdd from '@/views/work/WorkAdd.vue'
import WorkDelete from '@/views/work/WorkDelete.vue'
import WorkEdit from '@/views/work/WorkEdit.vue'
import WorkHourDelete from '@/views/work/WorkHourDelete.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomeView
    },
    {
      path: '/login',
      name: 'login',
      component: LoginView
    },
    {
      path: '/register',
      name: 'register',
      component: () => import('@/views/RegisterView.vue')
    },
    // ----Courses----
    {
      path: '/course',
      name: 'course',
      component: CoursesView
    },
    {
      path: '/course/create',
      name: 'coursecreate',
      component: () => import('@/views/courses/CourseCreate.vue')
    },
    {
      path: '/course/details/:id',
      name: 'coursedetails',
      component: CourseDetails,
      props: true
    },
    {
      path: '/course/delete/:id',
      name: 'coursedelete',
      component: CourseDelete,
      props: true
    },
    {
      path: '/course/edit/:id',
      name: 'courseedit',
      component: CourseEdit,
      props: true
    },
    // ----Hobbies----
    {
      path: '/hobby',
      name: 'hobby',
      component: HobbiesView
    },
    {
      path: '/hobby/create',
      name: 'hobbycreate',
      component: () => import('@/views/hobbies/HobbyCreate.vue')
    },
    {
      path: '/hobby/add/:id',
      name: 'hobbyadd',
      component: HobbyAdd,
      props: true
    },
    {
      path: '/hobby/delete/:id',
      name: 'hobbydelete',
      component: HobbyDelete,
      props: true
    },
    {
      path: '/hobby/edit/:id',
      name: 'hobbyedit',
      component: HobbyEdit,
      props: true
    },
    // ----Sleep----
    {
      path: '/sleep',
      name: 'sleep',
      component: SleepView
    },
    {
      path: '/sleep/create',
      name: 'sleepcreate',
      component: () => import('@/views/sleeps/SleepCreate.vue')
    },
    {
      path: '/sleep/delete/:id',
      name: 'sleepdelete',
      component: SleepDelete,
      props: true
    },
    {
      path: '/sleep/edit/:id',
      name: 'sleepedit',
      component: () => import('@/views/sleeps/SleepEdit.vue'),
      props: true
    },
    // ----Work----
    {
      path: '/work',
      name: 'work',
      component: WorkView
    },
    {
      path: '/work/create',
      name: 'workcreate',
      component: () => import('@/views/work/WorkCreate.vue')
    },
    {
      path: '/work/add/:id',
      name: 'workadd',
      component: WorkAdd,
      props: true
    },
    {
      path: '/work/delete/:id',
      name: 'workdelete',
      component: WorkDelete,
      props: true
    },
    {
      path: '/work/edit/:id',
      name: 'workedit',
      component: WorkEdit,
      props: true
    },
    {
      path: '/work/details/:id',
      name: 'workdetails',
      component: WorkHourDelete,
      props: true
    },
    {
      path: '/work/workhour/:id',
      name: 'workworkhourdelete',
      component: WorkHourDelete,
      props: true
    }
  ]
})

export default router

router.beforeEach(async (to) => {
  // redirect to login page if not logged in and trying to access a restricted page
  const publicPages = ['/login', '/register']
  const authRequired = !publicPages.includes(to.path)
  const auth = useAuthStore()

  if (authRequired && !auth.isAuthenticated) {
    auth.returnUrl = to.fullPath
    return '/login'
  }
})
