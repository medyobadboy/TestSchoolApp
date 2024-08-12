<template>
    <q-dialog ref="dialogRef" v-model="isOpen">
        <q-card class="q-dialog-plugin">
            <q-card-section>
                <strong v-if="id != null">Edit Course: {{ code }} {{ title }}</strong>
                <strong v-else>Add New Course:</strong>
            </q-card-section>
            <q-card-section>
                <q-form @submit="onSubmit" @reset="onReset" class="q-gutter-md">
                    <q-input filled v-model="code" label="Code *" lazy-rules
                        :rules="[val => val && val.length > 0 || 'Please type something']" />
                    <q-input filled v-model="title" label="Title *" lazy-rules
                        :rules="[val => val && val.length > 0 || 'Please type something']" />
                    <q-input filled v-model="credits" label="Credits *" type="number" lazy-rules
                        :rules="[val => val && val > 0 || 'Please type something']" />
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

export default {
    props: {
        data: {
            type: Object
        }
    },
    setup(props) {
        console.log(props.data)
        const id = ref(props.data.id)
        const code = ref(props.data.code)
        const title = ref(props.data.title)
        const credits = ref(props.data.credits)
        const isOpen = ref(true)

        return {
            id,
            code,
            title,
            credits,
            isOpen,

            onSubmit() {
                if (id.value == null) {
                    axios.post(import.meta.env.VITE_APP_API_URL + 'courses', {
                        id: 0,
                        code: code.value,
                        title: title.value,
                        credits: credits.value,
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
                    axios.put(import.meta.env.VITE_APP_API_URL + 'courses/' + id.value, {
                        id: id.value,
                        code: code.value,
                        title: title.value,
                        credits: credits.value,
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
                code.value = null
                title.value = null
                credits.value = null
            }
        }
    }
}
</script>