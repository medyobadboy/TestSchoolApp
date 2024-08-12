<template>
    <q-dialog ref="dialogRef" v-model="isOpen">
        <q-card class="q-dialog-plugin">
            <q-card-section>
                <strong v-if="id != null">Edit Application: {{ studentName }} - {{ code }}</strong>
                <strong v-else>Add New Application:</strong>
            </q-card-section>
            <q-card-section>
                <q-form @submit="onSubmit" @reset="onReset" class="q-gutter-md">
                    <!-- <q-input filled v-model="studentId" label="Student *" lazy-rules
                        :rules="[val => val && val.length > 0 || 'Please type something']" /> -->
                    <q-select filled v-model="studentId" :options="studentList" option-value="id"
                        option-label="fullName" label="Student" map-options />
                    <!-- <q-input filled v-model="courseId" label="Course *" lazy-rules
                        :rules="[val => val && val.length > 0 || 'Please type something']" /> -->
                    <q-select filled v-model="courseId" :options="courseList" option-value="id"
                        option-label="code" label="Course" map-options />
                    <q-input filled v-model="applicationDate" type="date" label="Application Date *" required lazy-rules
                        :rules="['Please select application date']" />
                    <div>
                        <q-btn label="Submit" type="submit" color="primary" />
                        <q-btn label="Reset" type="reset" color="primary" flat class="q-ml-sm" />
                    </div>
                </q-form>
            </q-card-section>
            <q-card-section>
                <slot name="cancelBtn" @click="handleCancelClick"></slot>
                <slot name="confirmBtn" @click="handleConfirmClick"></slot>
            </q-card-section>
        </q-card>
    </q-dialog>
</template>

<script>
import { date, useQuasar } from 'quasar'
import { ref } from 'vue'
import axios from 'axios'

const studentList = ref([])
const courseList = ref([])

export default {
    props: {
        data: {
            type: Object
        }
    },
    setup(props) {
        console.log(props.data)
        const id = ref(props.data.id)
        const studentName = props.data.student ? props.data.student.firstName + ' ' + props.data.student.lastName : null
        const code = props.data.course ? props.data.course.code : null
        const studentId = ref(props.data.student)
        const courseId = ref(props.data.course)
        const applicationDate = ref(props.data.applicationDate ? new Date(props.data.applicationDate).toISOString().slice(0, 10) : null)
        const isOpen = ref(true)

        return {
            id,
            studentName,
            code,
            studentId,
            courseId,
            applicationDate,
            isOpen,

            onSubmit() {
                if (id.value == null) {
                    axios.post(import.meta.env.VITE_APP_API_URL + 'applications', {
                        id: 0,
                        studentId: studentId.value.id,
                        courseId: courseId.value.id,
                        applicationDate: new Date(applicationDate.value),
                    })
                        .then((res) => {
                            console.log('success')
                            console.log(res.data)
                            isOpen.value = false
                        })
                        .catch((error) => {
                            console.log('error')
                            console.log(error)
                            isOpen.value = false
                        })
                }
                else {
                    debugger
                    axios.put(import.meta.env.VITE_APP_API_URL + 'applications/' + id.value, {
                        id: id.value,
                        studentId: studentId.value.id,
                        courseId: courseId.value.id,
                        applicationDate: new Date(applicationDate.value)
                    })
                        .then((res) => {
                            console.log('success')
                            console.log(res.data)
                            isOpen.value = false
                        })
                        .catch((error) => {
                            console.log('error')
                            console.log(error)
                            isOpen.value = false
                        })
                }
            },

            onReset() {
                studentId.value = null
                courseId.value = null
                applicationDate.value = null
            }
        }
    },
    data() {
        return {
            studentList: studentList,
            courseList: courseList
        }
    },
    methods: {
        async getStudentData() {
            const { data } = await axios.get(import.meta.env.VITE_APP_API_URL + "students");
            studentList.value = data;
        },
        async getCourseData() {
            const { data } = await axios.get(import.meta.env.VITE_APP_API_URL + "courses");
            courseList.value = data;
        }
    },
    beforeMount() {
        this.getStudentData();
        this.getCourseData();
    }
}
</script>