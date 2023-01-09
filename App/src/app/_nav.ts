import { INavData } from '@coreui/angular'

export const navItems: INavData[] = [
  {
    name: 'Dashboard',
    url: '/dashboard',
    icon: 'icon-speedometer',
    badge: {
      variant: 'info',
      text: 'NEW'
    }
  },

  {
    name: '0.系統',
    url: '/system',
    icon: 'icon-puzzle',
    children: [
      {
        name: '0.1 角色群組',
        url: '/system/account-group',
        icon: 'icon-puzzle'
      },
      {
        name: '0.2 帳號',
        url: '/system/account',
        icon: 'icon-puzzle'
      },

      // {
      //   name: '0.3 Period',
      //   url: '/system/period',
      //   icon: 'icon-puzzle'
      // },


    ]
  },
  {
    name: '1.基本資料',
    url: '/maintain',
    icon: 'icon-bell',
    children: [
      {
        name: '1.1 組織圖',
        url: '/system/oc',
        icon: 'icon-puzzle'
      },

      // {
      //   name: '1.2 項目',
      //   url: '/system/policy',
      //   icon: 'icon-puzzle'
      // },
      // {
      //   name: '1.3 建立KPI',
      //   url: '/system/kpi-create',
      //   icon: 'icon-puzzle'
      // },
      {
        name: '1.2 建立一階KPI',
        url: '/system/kpi-create2',
        icon: 'icon-puzzle'
      },
      {
        name: '1.3 建立二階&三階KPI',
        url: '/system/kpi-2nd3rd',
        icon: 'icon-puzzle'
      },
      {
        name: '1.4 查看所有KPI',
        url: '/system/view-kpi',
        icon: 'icon-puzzle'
      },
      {
        name: '1.5 設定更新月份',
        url: '/system/setting-monthly',
        icon: 'icon-puzzle'
      },
    ]
  },
  {
    name: '2.維護資料',
    url: '/transaction',
    icon: 'icon-cursor',
    children: [
      // {
      //   name: '2.1 KPI Objective',
      //   url: '/transaction/objective',
      //   icon: 'icon-cursor'
      // },
      // {
      //   name: '2.2 To Do List',
      //   url: '/transaction/todolist',
      //   icon: 'icon-cursor'
      // },
      {
        name: '2.1 更新PDCA',
        url: '/transaction/todolist2',
        icon: 'icon-cursor'
      },
      {
        name: '2.2 會議',
        url: '/transaction/meeting',
        icon: 'icon-cursor'
      },
    ]
  },
  {
    name: '3.看板',
    url: '/kanban',
    icon: 'icon-star',
    children: [

    ]
  },
   {
    name: '4.報表',
    url: '/report',
    icon: 'icon-calculator',
    children: [
      // {
      //   name: 'Q1,Q3 Report 季報表',
      //   url: '/report/q1-q3-report',
      //   icon: 'icon-calculator'
      // },
      // {
      //   name: 'H1 & H2 Report',
      //   url: '/report/h1-h2-report',
      //   icon: 'icon-calculator'
      // },
      // {
      //   name: 'HQ HR Report 年中考核名單',
      //   url: '/report/hq-hr-report',
      //   icon: 'icon-calculator'
      // },
      // {
      //   name: 'GHR Report ',
      //   url: '/report/ghr-report',
      //   icon: 'icon-calculator'
      // },
    ]
  }

];

export const navItemsVI: INavData[] = [
  {
    name: 'Dashboard',
    url: '/dashboard',
    icon: 'icon-speedometer',
    badge: {
      variant: 'info',
      text: 'NEW'
    }
  },

  {
    name: '0.System',
    url: '/system',
    icon: 'icon-puzzle',
    children: [
      {
        name: '0.1 Account Group',
        url: '/system/account-group',
        icon: 'icon-puzzle'
      },
      {
        name: '0.2 Account',
        url: '/system/account',
        icon: 'icon-puzzle'
      },

      // {
      //   name: '0.3 Period',
      //   url: '/system/period',
      //   icon: 'icon-puzzle'
      // },


    ]
  },
  {
    name: '1.Maintain',
    url: '/maintain',
    icon: 'icon-bell',
    children: [
      {
        name: '1.1 OC',
        url: '/system/oc',
        icon: 'icon-puzzle'
      },

      {
        name: '1.2 Policy',
        url: '/system/policy',
        icon: 'icon-puzzle'
      },
      // {
      //   name: '1.3 KPI Create',
      //   url: '/system/kpi-create',
      //   icon: 'icon-puzzle'
      // },

      {
        name: '1.3 KPI Create',
        url: '/system/kpi-create2',
        icon: 'icon-puzzle'
      },
      {
        name: '1.4 KPI 2nd & 3rd Create',
        url: '/system/kpi-2nd3rd',
        icon: 'icon-puzzle'
      },
      {
        name: '1.5 View KPI',
        url: '/system/view-kpi',
        icon: 'icon-puzzle'
      },
      {
        name: '1.6 Monthly Setting',
        url: '/system/setting-monthly',
        icon: 'icon-puzzle'
      },
    ]
  },
  {
    name: '2.Transaction',
    url: '/transaction',
    icon: 'icon-cursor',
    children: [
      // {
      //   name: '2.1 KPI Objective',
      //   url: '/transaction/objective',
      //   icon: 'icon-cursor'
      // },
      // {
      //   name: '2.2 To Do List',
      //   url: '/transaction/todolist',
      //   icon: 'icon-cursor'
      // },
      {
        name: '2.1 To Do List',
        url: '/transaction/todolist2',
        icon: 'icon-cursor'
      },
      {
        name: '2.2 Meeting',
        url: '/transaction/meeting',
        icon: 'icon-cursor'
      },
    ]
  },
  {
    name: '3.Kanban',
    url: '/kanban',
    icon: 'icon-star',
    children: [

    ]
  },
   {
    name: '4.Report',
    url: '/report',
    icon: 'icon-calculator',
    children: [
      // {
      //   name: 'Q1,Q3 Report 季報表',
      //   url: '/report/q1-q3-report',
      //   icon: 'icon-calculator'
      // },
      // {
      //   name: 'H1 & H2 Report',
      //   url: '/report/h1-h2-report',
      //   icon: 'icon-calculator'
      // },
      // {
      //   name: 'HQ HR Report 年中考核名單',
      //   url: '/report/hq-hr-report',
      //   icon: 'icon-calculator'
      // },
      // {
      //   name: 'GHR Report ',
      //   url: '/report/ghr-report',
      //   icon: 'icon-calculator'
      // },
    ]
  }

];

export const navItemsEN: INavData[] = [
  {
    name: 'Dashboard',
    url: '/dashboard',
    icon: 'icon-speedometer',
    badge: {
      variant: 'info',
      text: 'NEW'
    }
  },

  {
    name: '0.System',
    url: '/system',
    icon: 'icon-puzzle',
    children: [
      {
        name: '0.1 Account Group',
        url: '/system/account-group',
        icon: 'icon-puzzle'
      },
      {
        name: '0.2 Account',
        url: '/system/account',
        icon: 'icon-puzzle'
      },

      // {
      //   name: '0.3 Period',
      //   url: '/system/period',
      //   icon: 'icon-puzzle'
      // },


    ]
  },
  {
    name: '1.Maintain',
    url: '/maintain',
    icon: 'icon-bell',
    children: [
      {
        name: '1.1 OC',
        url: '/maintain/oc',
        icon: 'icon-puzzle'
      },

      // {
      //   name: '1.2 Policy',
      //   url: '/system/policy',
      //   icon: 'icon-puzzle'
      // },
      // {
      //   name: '1.3 KPI Create',
      //   url: '/system/kpi-create',
      //   icon: 'icon-puzzle'
      // },
      {
        name: '1.2 KPI Create',
        url: '/maintain/kpi-create2',
        icon: 'icon-puzzle'
      },
      {
        name: '1.3 KPI 2nd & 3rd Create',
        url: '/maintain/kpi-2nd3rd',
        icon: 'icon-puzzle'
      },
      {
        name: '1.4 View KPI',
        url: '/maintain/view-kpi',
        icon: 'icon-puzzle'
      },
      {
        name: '1.5 Monthly Setting',
        url: '/maintain/setting-monthly',
        icon: 'icon-puzzle'
      },
      {
        name: '1.6 Role',
        url: '/maintain/role',
        icon: 'icon-puzzle'
      },
    ]
  },
  {
    name: '2.Transaction',
    url: '/transaction',
    icon: 'icon-cursor',
    children: [

      {
        name: '2.1 To Do List',
        url: '/transaction/todolist2',
        icon: 'icon-cursor'
      },
      {
        name: '2.2 Meeting',
        url: '/transaction/meeting',
        icon: 'icon-cursor'
      },
      {
        name: '2.3 Change Password',
        url: '/transaction/change-password',
        icon: 'icon-cursor'
      },
    ]
  },
  {
    name: '3.Kanban',
    url: '/kanban',
    icon: 'icon-star',
    children: [

    ]
  },
   {
    name: '4.Report',
    url: '/report',
    icon: 'icon-calculator',
    children: [
      // {
      //   name: 'Q1,Q3 Report 季報表',
      //   url: '/report/q1-q3-report',
      //   icon: 'icon-calculator'
      // },
      // {
      //   name: 'H1 & H2 Report',
      //   url: '/report/h1-h2-report',
      //   icon: 'icon-calculator'
      // },
      // {
      //   name: 'HQ HR Report 年中考核名單',
      //   url: '/report/hq-hr-report',
      //   icon: 'icon-calculator'
      // },
      // {
      //   name: 'GHR Report ',
      //   url: '/report/ghr-report',
      //   icon: 'icon-calculator'
      // },
    ]
  }

];
