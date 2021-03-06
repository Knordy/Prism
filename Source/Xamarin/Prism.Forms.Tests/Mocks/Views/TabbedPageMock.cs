﻿using Prism.Mvvm;
using Prism.Navigation;
using Xamarin.Forms;

namespace Prism.Forms.Tests.Mocks.Views
{
    public class TabbedPageMock : TabbedPage, IDestructible, INavigationAware, IPageNavigationEventRecordable
    {
        public bool DestroyCalled { get; private set; } = false;
        public PageNavigationEventRecorder PageNavigationEventRecorder { get; set; }

        public TabbedPageMock() : this(null)
        {
        }

        public TabbedPageMock(PageNavigationEventRecorder recorder)
        {
            ViewModelLocator.SetAutowireViewModel(this, true);

            Children.Add(new ContentPageMock(recorder) { Title = "Page 1" });
            Children.Add(new PageMock() { Title = "Page 2", BindingContext = null });
            Children.Add(new ContentPageMock(recorder) { Title = "Page 3" });
            Children.Add(new NavigationPageMock(recorder, new ContentPageMock(recorder)) { Title = "Page 4" });
            Children.Add(new NavigationPageMock(recorder, new PageMock()) { Title = "Page 5" });

            PageNavigationEventRecorder = recorder;

            var recordable = BindingContext as IPageNavigationEventRecordable;
            if (recordable != null)
                recordable.PageNavigationEventRecorder = recorder;
        }


        public void Destroy()
        {
            DestroyCalled = true;
            PageNavigationEventRecorder?.Record(this, PageNavigationEvent.Destroy);
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
            PageNavigationEventRecorder?.Record(this, PageNavigationEvent.OnNavigatedFrom);
        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            PageNavigationEventRecorder?.Record(this, PageNavigationEvent.OnNavigatedTo);
        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {
            PageNavigationEventRecorder?.Record(this, PageNavigationEvent.OnNavigatingTo);
        }
    }
}
