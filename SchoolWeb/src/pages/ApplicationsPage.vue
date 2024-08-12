<template>
    <!-- <h5>Applications</h5> -->
    <div class="q-pa-md">
        <q-btn @click="newRecord()" icon="add_circle_outline"></q-btn>
        <q-table title="Applications" :rows="rows" :columns="columns" row-key="name"
            no-data-label="I didn't find anything for you">
            <template v-slot:body="props">
                <q-tr :props="props" @click="onRowClick(props.row)">
                    <q-td v-for="col in columns" :key="col.name" :props="props">
                        <q-btn v-if="col.name == 'buttons'" icon="delete"
                            @click.stop="deleteData(props.row.id)"></q-btn>
                        <span v-else-if="col.name == 'studentId'">{{ props.row.student.firstName }} {{ props.row.student.lastName }}</span>
                        <span v-else-if="col.name == 'courseId'">{{ props.row.course.code }}</span>
                        <span v-else-if="col.name == 'applicationDate'">{{ (new Date(props.row[col.name])).toLocaleDateString() }}</span>
                        <span v-else>{{ props.row[col.name] }}</span>
                    </q-td>
                </q-tr>
            </template>
        </q-table>
    </div>
</template>

<script>
import { ref } from 'vue'
import axios from "axios";
import { date, useQuasar } from 'quasar'
import EditApplication from "src/components/EditApplication.vue";

const columns =
    [
        { name: 'id', label: "Id", field: row => row.id },
        { name: 'studentId', label: "Student Name" },
        { name: 'courseId', label: "Course Code" },
        { name: 'applicationDate', label: "Application Date" },
        { name: 'buttons', label: '', field: row => row.id }
    ]
const rows = ref([])
// const rows = await axios.get("http://localhost:5260/api/Applications")

export default {
    setup() {
        const $q = useQuasar()

        const getData = async () => {
            console.log(import.meta.env.VITE_APP_API_URL)
            const { data } = await axios.get(import.meta.env.VITE_APP_API_URL + "applications");
            // console.log(data);
            rows.value = data;
        }
        function onRowClick(row) {
            // console.log('ROW:')
            console.log(row)
            $q.dialog({ component: EditApplication, componentProps: { data: row } }).onDismiss(() => getData())
        }

        function newRecord() {
            $q.dialog({ component: EditApplication, componentProps: { data: {} } }).onDismiss(() => getData())
        }
        return {
            onRowClick,
            newRecord,
            getData
        }
    },
    data() {

        return {
            rows: rows,
            columns: columns
        };
    },
    methods: {
        async deleteData(id) {
            await axios.delete(import.meta.env.VITE_APP_API_URL + 'applications/' + id)
                .then((res) => {
                    console.log('success')
                    console.log(res.data)
                })
                .catch((error) => {
                    console.log('error')
                    console.log(error)
                })
            this.getData();
        }

    },
    beforeMount() {
        this.getData();
    },
}
</script>