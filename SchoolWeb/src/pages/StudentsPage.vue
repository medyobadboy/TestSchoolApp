<template>
    <!-- <h5>Students</h5> -->
    <div class="q-pa-md">
        <q-btn @click="newRecord()" icon="add_circle_outline"></q-btn>
        <q-table title="Students" :rows="rows" :columns="columns" row-key="name"
            no-data-label="I didn't find anything for you">
            <template v-slot:body="props">
                <q-tr :props="props" @click="onRowClick(props.row)">
                    <q-td v-for="col in columns" :key="col.name" :props="props">
                        <q-btn v-if="col.name == 'buttons'" icon="delete" @click.stop="deleteData(props.row.id)"></q-btn>
                        <span v-else>{{ col.name == 'dateOfBirth' ? (new Date(props.row[col.name])).toLocaleDateString()
                            : props.row[col.name]}}</span>
                    </q-td>
                </q-tr>
            </template>
        </q-table>
    </div>
</template>

<script>
import {ref} from 'vue'
import axios from "axios";
import { date, useQuasar } from 'quasar'
import EditStudent from "src/components/EditStudent.vue";

const columns =
    [
        { name: 'id', label: "Id", field: row => row.id },
        { name: 'firstName', label: "First Name", field: row => row.firstName },
        { name: 'lastName', label: "Last Name", field: row => row.lastName },
        { name: 'dateOfBirth', label: "Date of Birth", field: row => row.dateOfBirth, type: date },
        { name: 'email', label: "Email", field: row => row.email },
        { name: 'phoneNumber', label: "Phone Number", field: row => row.phoneNumber },
        { name: 'buttons', label: '', field: row => row.id }
    ]
const rows = ref([])
// const rows = await axios.get("http://localhost:5260/api/Students")

export default {
    setup() {
        const $q = useQuasar()

        const getData = async () => {
            console.log(import.meta.env.VITE_APP_API_URL)
            const { data } = await axios.get(import.meta.env.VITE_APP_API_URL + "students");
            // console.log(data);
            rows.value = data;
        }
        function onRowClick(row) {
            // console.log('ROW:')
            // console.log(row)
            $q.dialog({ component: EditStudent, componentProps: { data: row } }).onDismiss(() => getData())
        }

        function newRecord() {
            $q.dialog({ component: EditStudent, componentProps: { data: {} } }).onDismiss(() => getData())
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
            await axios.delete(import.meta.env.VITE_APP_API_URL + 'students/' + id)
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