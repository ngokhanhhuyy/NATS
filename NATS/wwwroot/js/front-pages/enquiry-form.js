import { createApp, ref, reactive, computed, onMounted, watch } from "vue";
import { Modal } from "bootstrap";

/**
 * @typedef {"notSubmitted" | "submitting" | "submitted"} SubmissionState
 * @typedef {{
 *     fullName: string;
 *     phoneNumber: string;
 *     email: string;
 *     content: string;
 * }} Model
 * 
 * @typedef {{
 *     isMounted: boolean;
 *     model: Model;
 *     errors: Map<string, string>;
 *     isValidatedStates: IsValidatedStates;
 *     submissionState: SubmissionState;
 * }} States
 * 
 * @typedef {{
 *     displayName: string;
 *     isRequired: boolean;
 *     minLength?: number;
 *     regexPattern?: RegExp;
 * }} ValidationRule
 * 
 * @typedef {{ [key: typeof Model]: ValidationRule }} ValidationRules
 * @typedef {{ [key: typeof Model]: boolean }} IsValidatedStates
 */

/** @type {ValidationRules} */
const validationRules = {
    fullName: {
        displayName: "Tên đầy đủ",
        isRequired: true,
        minLength: 1
    },
    phoneNumber: {
        displayName: "Số điên thoại",
        isRequired: true,
        minLength: 10,
        regexPattern: /[0-9+]+/
    },
    email: {
        displayName: "Địa chỉ email",
        isRequired: false,
        regexPattern: /^[\w-.]+@([\w-]+\.)+[\w-]{2,4}$/
    },
    content: {
        displayName: "Nội dung",
        isRequired: true,
        minLength: 20
    }
};

createApp({
    setup() {
        // States.
        /** @type {States} */
        const states = reactive({
            isMounted: false,
            model: {
                fullName: "",
                phoneNumber: "",
                email: "",
                content: ""
            },
            errors: new Map(),
            isValidatedStates: {
                fullName: false,
                phoneNumber: false,
                email: false,
                content: false
            },
            submissionState: "notSubmitted"
        });
        
        /** @type {import("vue").Ref<HTMLDivElement | undefined>} */
        const modalElement = ref();
        
        /** @type {import("vue").Ref<Modal | undefined>} */
        const modal = ref();
        
        // Computed properties.
        /** @type {import("vue").ComputedRef<boolean>} */
        const areAllFieldsValid = computed(() => {
            const invalidFieldCount = Object
                .keys(states.model)
                .map(key => {
                    if (key !== "email") {
                        return isValid(key) ?? false;
                    }
                    
                    if (states.model.email.length === 0) {
                        return true;
                    }
                    
                    return isValid(key);
                }).filter(isValid => isValid != null && !isValid)
                .length;
            
            return invalidFieldCount === 0;
        });

        // Hooks.
        onMounted(() => {
            states.isMounted = true;
            if (modalElement.value) {
                modal.value = new Modal(modalElement.value);
            }
        });

        // Watch.
        watch(() => states.model, (/** @type {Model} */ model) => {
            for (const key of Object.keys(model)) {
                /** @type {string} */
                const value = model[key];
                
                /** @type {ValidationRule} */
                const rule = validationRules[key];
                
                if (value.length) {
                    states.isValidatedStates[key] = true;
                }
                
                if (states.isValidatedStates[key]) {
                    if (rule.isRequired && !value.length) {
                        states.errors.set(key, `${rule.displayName} không được để trống.`);
                        continue;
                    }
                    
                    if (rule.minLength && value.length < rule.minLength) {
                        states.errors.set(
                            key,
                            `${rule.displayName} phải chứa ít nhất ${rule.minLength} ký tự.`);
                        continue;
                    }
                    
                    console.log(key, rule.regexPattern);
                    if (rule.regexPattern && !rule.regexPattern.test(value)) {
                        states.errors.set(key, `${rule.displayName} không hợp lệ.`);
                        continue;
                    }
                    
                    states.errors.delete(key);
                }
            }
        }, { deep: true });

        // Functions.
        /**
         * Checks if field is valid.
         * 
         * @param key
         * @returns {boolean | null}
         */
        function isValid(/** @type {string} */ key) {
            if (!states.isValidatedStates[key]) {
                return null;
            }

            return !states.errors.get(key);
        }

        /**
         * Gets the classname of the input element corresponding to a field in the model, based
         * on its validation state, specified by the field's name.
         * 
         * @param {string} key The key of the corresponding field in the model.
         * @returns {string | undefined} A `string` representing the classname of the input
         * element if the field has been validated. Otherwise, `undefined`.
         */
        function getInputClassName(key) {
            const isFieldValid = isValid(key);
            if (isFieldValid == null) {
                return;
            }

            if (!isFieldValid) {
                return "is-invalid";
            }

            return "is-valid";
        }

        /**
         * Gets the classname of the validation message element corresponding to a field in the
         * model, based on its validation state, specified by the field's name.
         * 
         * @param {string} key The key of the corresponding field in the model.
         * @returns {string | undefined} A `string` representing the classname of the
         * validation message element if the field has been validated. Otherwise, `undefined`.
         */
        function getMessageClassName(key) {
            const isFieldValid = isValid(key);
            if (isFieldValid == null) {
                return;
            }

            if (!isFieldValid) {
                return "text-danger";
            }

            return "text-success";
        }

        /**
         * Gets the validation message of a field in the model, based on its validation state,
         * specified by the field's name.
         * 
         * @param {string} key The key of the corresponding field in the model.
         * @returns {string | undefined} A `string` representing the validation message if the
         * field has been validated. Otherwise, `undefined`.
         */
        function getMessage(key) {
            const isFieldValid = isValid(key);
            if (isFieldValid != null) {
                return states.errors.get(key);
            }
        }

        /**
         * Handles form submission event.
         * 
         * @param {SubmitEvent} event The event emitted by the form when submitted.
         */
        function handleFormSubmitting(event) {
            event.preventDefault();
            states.submissionState = "submitting";
            setTimeout(() => {
                if (modal.value) {
                    modal.value.show();
                }
                states.submissionState = "submitted";
            }, 1000);
        }
        
        return {
            states,
            modal,
            modalElement,
            getInputClassName,
            getMessageClassName,
            getMessage,
            areAllFieldsValid,
            handleFormSubmitting
        };
    }
}).mount("#vue-app");