#pragma once

#include "defines.h"

typedef void (STDCALL * REGISTER_COMMAND_HANDLER_CALLBACK)(const uint16_t *event_name, void *handler_handle);
typedef void (STDCALL * REVERSE_COMMAND_CALLBACK)(const uint16_t *command_name, const uint16_t *command_arguments);
typedef void * (STDCALL * LOAD_MODULE_CALLBACK)(const uint16_t *module_name);
typedef void (STDCALL * LOG_CALLBACK)(const uint16_t *message);
typedef void (STDCALL * REPORT_ERROR_CALLBACK)(const int error_code, const uint16_t *error_message);

extern "C" 
{
	JS1_API int js1_api_version();
	JS1_API void * STDCALL compile_module(void *prelude, const uint16_t *module, const uint16_t *file_name);
	JS1_API void * STDCALL compile_prelude(const uint16_t *prelude, const uint16_t *file_name, LOAD_MODULE_CALLBACK load_module_callback, LOG_CALLBACK log_callbck);
	JS1_API void * STDCALL compile_query(
		void *prelude, 
		const uint16_t *script,
		const uint16_t *file_name,
		REGISTER_COMMAND_HANDLER_CALLBACK register_command_handler_callback,
		REVERSE_COMMAND_CALLBACK reverse_command_callback
	);

	JS1_API void STDCALL dispose_script(void *script_handle);

	JS1_API void * STDCALL execute_command_handler(void *script_handle, void *event_handler_handle, const uint16_t *data_json, const uint16_t *data_other[], int32_t other_length, uint16_t **result_json);

	JS1_API void STDCALL free_result(void *result);

	JS1_API void report_errors(void *script_handle, REPORT_ERROR_CALLBACK report_error_callback);
}
