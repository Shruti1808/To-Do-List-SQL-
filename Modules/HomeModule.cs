using Nancy;
using System;
using System.Collections.Generic;
using Nancy.ViewEngines.Razor;

namespace ToDoListSql
{
  public class HomeModule: NancyModule
  {
    public HomeModule()
    {
      Get["/"] = _ => {
        List<Category> AllCategories = Category.GetAll();
        return View["index.cshtml", AllCategories];
      };

      Get["/tasks"] = _ => {
        List<Task> AllTasks = Task.GetAll();
        return View["tasks.cshtml", AllTasks];
      };

      Get["/categories"] = _ => {
        List<Category> AllCategories = Category.GetAll();
        return View["categories.cshtml", AllCategories];
      };

      Get["/categories/new"] = _ => {
        return View["categories_form.cshtml"];
      };

      Post["/categories/new"] = _ => {
        Category newCategory = new Category(Request.Form["category-name"]);
        newCategory.Save();
        return View["success.cshtml"];
      };

      Get["/tasks/new"] = _ => {
        return View["tasks_form.cshtml"];
      };

      Post["/tasks/new"] = _ => {
        Task newTask = new Task(Request.Form["task-description"], Request.Form["due-date"]);
        newTask.Save();
        return View["success.cshtml"];
      };

      Get["tasks/{id}"] = parameters => {
        Dictionary<string, object> model = new Dictionary<string, object>();
        Task SelectedTask = Task.Find(parameters.id);
        List<Category> TaskCategories = SelectedTask.GetCategories();
        List<Category> AllCategories = Category.GetAll();
        model.Add("task", SelectedTask);
        model.Add("taskCategories", TaskCategories);
        model.Add("allCategories", AllCategories);
        return View["task.cshtml", model];
      };

      Get["/categories/{id}"] = parameters => {
        Dictionary<string, object> model = new Dictionary<string, object>();
        var SelectedCategory = Category.Find(parameters.id);
        var CategoryTasks = SelectedCategory.GetTasks();
        model.Add("category", SelectedCategory);
        model.Add("tasks", CategoryTasks);
        return View["category.cshtml", model];
      };
      //To Add A Category
      Post["task/add_category"] = _ => {
        Category category = Category.Find(Request.Form["category-id"]);
        Task task = Task.Find(Request.Form["task-id"]);
        task.AddCategory(category);
        return View["success.cshtml"];
      };
      Post["category/add_task"] = _ => {
        Category category = Category.Find(Request.Form["category-id"]);
        Task task = Task.Find(Request.Form["task-id"]);
        category.AddTask(task);
        return View["success.cshtml"];
      };

      Post["/categories/delete"] = _ => {
        Category.DeleteAll();
        Task.DeleteAll();
        return View["success.cshtml"];
      };

      Post["/tasks/delete"] = _ => {
        Task.DeleteAll();
        return View["success.cshtml"];
      };

      Post["/categories/{id}/delete"] = parameters => {
        Category.Find(parameters.id).DeleteCategory();
        return View["success.cshtml"];
      };

    }
  }
}
