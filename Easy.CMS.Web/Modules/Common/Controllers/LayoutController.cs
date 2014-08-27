﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Easy.Web.Controller;
using Easy.CMS.Layout;
using Easy.Web.Attribute;
using Easy.Extend;
using Easy.Constant;
using Easy.CMS.Zone;
using Easy.CMS.Common.ViewModels;
using Easy.CMS.Widget;

namespace Easy.CMS.Common.Controllers
{

    public class LayoutController : BasicController<LayoutEntity, LayoutService>
    {
        [AdminTheme]
        public override System.Web.Mvc.ActionResult Index(ParamsContext context)
        {
            return View(Service.Get(new Data.DataFilter()));
        }
        [AdminTheme]
        public ActionResult LayoutWidget()
        {
            return View(Service.Get());
        }
        [HttpPost]
        public ActionResult LayoutZones(string ID)
        {
            ZoneService zoneService = new ZoneService();
            WidgetService  widgetService=new WidgetService();
            LayoutZonesViewModel viewModel = new LayoutZonesViewModel
            {
                Zones = zoneService.GetZonesByLayoutId(ID),
                Widgets = widgetService.GetByLayoutId(ID)
            };
            return View(viewModel);
        }
        [AdminTheme]
        public override ActionResult Create(ParamsContext context)
        {
            return base.Create(context);
        }
        [AdminTheme]
        public override ActionResult Create(LayoutEntity entity)
        {
            return base.Create(entity);
        }
        [AdminTheme]
        public override ActionResult Edit(ParamsContext context)
        {
            return base.Edit(context);
        }
        [AdminTheme]
        [HttpPost]
        public override ActionResult Edit(LayoutEntity entity)
        {
            if (entity.ActionType == ActionType.Design)
            {
                return RedirectToAction("Design", new { ID = entity.ID });
            }
            return base.Edit(entity);
        }
        public ActionResult Design(ParamsContext context)
        {
            LayoutEntity layout = null;
            if (!context.ID.IsNullOrEmpty())
            {
                layout = new LayoutService().Get(context.ID);
            }
            return View(layout ?? new LayoutEntity());
        }

        [ValidateInput(false)]
        public ActionResult SaveLayout(string[] html, LayoutEntity layout)
        {
            LayoutHtmlCollection htmls;
            var zones = Easy.CMS.Zone.Helper.GetZones(html, out htmls);
            layout.Zones = zones;
            layout.Html = htmls;
            Service.UpdateDesign(layout);
            return RedirectToAction("Edit", new { ID = layout.ID, module = "Common" });
        }
    }
}