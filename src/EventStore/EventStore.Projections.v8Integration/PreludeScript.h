#pragma once
#include "js1.h"
#include "CompiledScript.h"
#include "QueryScript.h"
#include "ModuleScript.h"

namespace js1 {
	class ModuleScript;

	class PreludeScript : public CompiledScript 
	{
	public:
		PreludeScript(LOAD_MODULE_CALLBACK load_module_callback_, LOG_CALLBACK log_callback_) :
			isolate(v8::Isolate::New()), load_module_handler(load_module_callback_), log_handler(log_callback_) 
		{
			isolate_add_ref(isolate);
		}

		virtual ~PreludeScript();
		bool compile_script(const uint16_t *prelude_source, const uint16_t *prelude_file_name);
		bool run();
		v8::Persistent<v8::ObjectTemplate> get_template(std::vector<v8::Handle<v8::Value> > &prelude_arguments);
	protected:
		virtual v8::Isolate *get_isolate();
		virtual v8::Persistent<v8::ObjectTemplate> create_global_template();
 
	private:
		v8::Isolate *isolate;
		v8::Persistent<v8::Function> global_template_factory;
		LOAD_MODULE_CALLBACK load_module_handler;
		LOG_CALLBACK log_handler;
		ModuleScript *load_module(uint16_t *module_name);

		static v8::Handle<v8::Value> log_callback(const v8::Arguments& args); 
		static v8::Handle<v8::Value> load_module_callback(const v8::Arguments& args); 
	};


}
