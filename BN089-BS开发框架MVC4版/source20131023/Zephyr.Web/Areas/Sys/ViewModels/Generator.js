/**
* 模块名：mms viewModel
* 程序名: Generator.js
* Copyright(c) 2013-2015 liuhuisheng [ liuhuisheng.xm@gmail.com ] 
**/

var viewModel = function () {
    var self = this;

    this.form = {
        type: '',
        database:ko.observable(),
        table: ko.observable(),
        controller: ko.observable(),
        area:ko.observable(),
        conditions: ko.observableArray(),
        columns: ko.observableArray(),
        tabs: ko.observableArray(),
        path: ko.observable("~/Generator/")
    };
 
    this.resetForm = function () {
        self.form.conditions([]);
        self.form.columns([]);
        self.form.tabs([]);
    };

    this.data = {
        codetype: [{ text: 'search', value: 'search' }, { text: 'edit', value: 'edit' }, { text: 'searchEdit', value: 'searchEdit' }],
        database: ko.observableArray(), // [{ text: 'Zephyr.Mms', value: 'Mms' }, { text: 'Zephyr.Sys', value: 'Sys' }],
        table: ko.observableArray(),
        column:ko.observableArray(),
        tablekey: ko.observableArray(),
        input: ['text', 'autocomplete', 'combobox', 'lookup','datebox','daterange'],
        compare: ['equal', 'like', 'startwith', 'endwith', 'greater', 'less', 'daterange'],
        align:['left','center','right'],
        formatter: [{text:'',value:''},{ text: '日期', value: 'com.formatDate' }, { text: '时间', value: 'com.formatTime' }, { text: '金额', value: 'com.formatMoney' }, { text: '是否', value: 'com.formatCheckbox' }],
        editor: [{text:'',value:''},{ text: '文本', value: 'text'}, { text: '整数', value: "{type: 'numberbox',options:{min: 0}}" }, { text: '两位小数', value: "{type: 'numberbox',options:{min: 0, precision: 2}}" }, { text: '下拉框', value: "{type:'combobox',options:{}}" }, { text: '弹出框', value: "{type:'lookup',options:{}}" }, { text: '日期', value: 'datebox' }]
    };

    this.initDatabase = function () {
        com.ajax({
            type: 'GET',
            async:false,//chrome执行过快时，出现未绑定的情况，改为同步
            url: '/api/sys/generator/GetConnectionStrings',
            success: function (d) {
                self.data.database(d);
            }
        });
    };

    this.initDatabase();

    this.getTableUrl = function () {
        return '/api/sys/generator/GetTables?database=' + self.form.database();
    };
    this.getColumnUrl = function (table) {
        return '/api/sys/generator/GetColumns?database=' + self.form.database() + "&table=" + table;
    }

    this.codetype = {
        showblank: true,
        width: 110,
        data: self.data.codetype,
        onSelect: function (node) {
            self.form.type = node.value;
            self.initWizard();
            //self.searchEdit.columntree2.url = ko.computed(function (){return '/api/sys/generator/GetColumns?database=' + self.form.database() + '&table=' + self.form.table();})
        }
    };

    this.database = {
        showblank: true,
        width: 110,
        data: self.data.database,
        onSelect: function (node) {
            self.form.database(node.value)
            //if (self.form.database)
            //    self.tabletree.url('/api/sys/generator/GetTables?database=' + self.form.database);
            self.form.area((node.value.split('.')[1] || node.value).replace(/(^|\s+)\w/g, function (s) { return s.toUpperCase(); }));
        }
    };

    this.tabletree = {
        method: 'GET',
        url: ko.computed(self.getTableUrl),
        loadFilter: function (d) {
            var data = utils.filterProperties(d.rows || d, ['TableName as id', 'TableName as text']);
            self.data.table(data);
            return data;
        },
        onSelect: function (node) {
            self.form.table(node.id);
            self.edit.init();
            self.resetWizard();

            //self.searchEdit.columntree.url('/api/sys/generator/GetColumns?database=' + self.form.database + '&table=' + self.form.table);
            //self.searchEdit.columntree2.url('/api/sys/generator/GetColumns?database=' + self.form.database + '&table=' + self.form.table);
            self.form.controller((node.id.split('_')[1] || node.id).replace(/(^|\s+)\w/g, function (s) { return s.toUpperCase(); }));
        }
    };

    this.generator = function () {
        com.ajax({
            type:'POST',
            url: '/api/sys/generator',
            data: ko.toJSON(self.form),
            success: function (d) {
                com.message('success', "代码已生成！");
            }
        });
    };

    this.searchEdit = {};
    this.searchEdit.columntree = {
        method: 'GET',
        url: ko.computed(function () {
            return self.getColumnUrl(self.form.table());
        }),
        checkbox: true,
        loadFilter: function (d) {
            return utils.filterProperties(d.rows || d, ['ColumnName as id', 'ColumnName as text']);
        },
        onSelect: function (node) {
            var handle = node.checked ? 'uncheck' : 'check';
            $(this).tree(handle, node.target);
        },
        onCheck: function (node, checked) {
            if (checked)
                self.form.conditions.push({ field: node.id, title: node.id, type: 'text', options: '', cp: 'equal',readonly:false });
            else
                self.form.conditions.remove(function (item) { return item.field == node.id });
        },
        onLoadSuccess: self.resetForm
    };

    this.searchEdit.columntree2 = {
        method: 'GET',
        url: ko.computed(function () {
            return self.getColumnUrl(self.form.table());
        }),
        checkbox: true,
        loadFilter: function (d) {
            return utils.filterProperties(d.rows || d, ['ColumnName as id', 'ColumnName as text']);
        },
        onSelect: function (node) {
            var handle = node.checked ? 'uncheck' : 'check';
            $(this).tree(handle, node.target);
        },
        onCheck: function (node, checked) {
            var arr = self.form.columns;
            
            if (checked) {
                var item = $.grep(arr(), function (row) {return row.field == node.id;})[0];
                item || arr.push({ field: node.id, title: node.id, hidden: false, sortable: true, align: 'left', width: 80, formatter: '', editor: 'text' });
            } else
                arr.remove(function (item) { return item.field == node.id });
        }
    };

    this.edit = {};
    this.edit.selectedTab = {
        title: ko.observable(),
        type: ko.observable(),
        subtable: ko.observable(),
        relationship: ko.observable(),
        columns: ko.observableArray(),
        primaryKeys:ko.observableArray()
    };
     
    this.edit.columntree2 = {
        method: 'GET',
        url:ko.observable(),
        checkbox: true,
        loadFilter: function (d) {
            self.data.column(d);
            var list = utils.filterProperties(d.rows || d, ['ColumnName as id', 'ColumnName as text']);
            self.edit.setDefaultForm();
            self.edit.resetTableKey();
            var checkedList = [];
            for (var i in self.edit.selectedTab.columns())
                checkedList.push(self.edit.selectedTab.columns()[i].field);
            for (var i in list)
                if ($.inArray(list[i].id, checkedList) > -1) list[i].checked = true;
            
            return list
        },
        onSelect: function (node) {
            var handle = node.checked ? 'uncheck' : 'check';
            $(this).tree(handle, node.target);
        },
        onCheck: function (node, checked) {
            var arr = self.edit.selectedTab.columns;

            if (checked) {
                var item = $.grep(arr(), function (row) { return row.field == node.id; })[0];
                item || arr.push({ field: node.id, title: node.id, hidden: false, sortable: true, align: 'left', width: 80, formatter: '', editor: 'text', type: '', readonly: true });
            } else
                arr.remove(function (item) { return item.field == node.id });
        }
    }
    this.edit.init = function () {
        self.edit.selectedTitle(null);
        self.edit.selectedTab = null;
        $('#edit-tab-setting').empty();
    };
    this.edit.addTab = function () {
        var title = 'tab' + (self.form.tabs().length + 1);
        var newTab = {
            title: ko.observable(title),
            type: ko.observable('empty'),
            subtable: ko.observable(self.form.table()),
            relationship: ko.observable(),
            columns: ko.observableArray(),
            primaryKeys:ko.observableArray()
        };
        newTab.type.subscribe(function (value) {
            if (value == 'grid') {
                var item = $.grep(self.data.table(), function (row) { return row.id == self.form.table() + "Detail" })[0];
                if (item)
                    newTab.subtable(item.id);
            }
            else if (value == 'form') {
                newTab.subtable(self.form.table());
            }
        });
        newTab.columns.subscribe(self.tableDnDUpdate);
        newTab.subtable.subscribe(function (value) {
            self.edit.selectedTab.columns([]);
            self.edit.columntree2.url(self.getColumnUrl(value));
        });

        self.form.tabs.push(newTab);
    };
    
    this.edit.removeTab = function (row,event) {
        self.form.tabs.remove(row);

        if (row.title() == self.edit.selectedTitle())
            self.edit.selectedTitle(null);
    };
    this.edit.selectedTitle = ko.observable();
    this.edit.clickTab = function (row, event) {
        if (row.title() == self.edit.selectedTitle()) return;
 
        self.edit.selectedTitle(row.title());
        self.edit.selectedTab = row;
        self.edit.columntree2.url = ko.observable(self.getColumnUrl(self.edit.selectedTab.subtable()));

        var currentTr = $(event.srcElement).parent("td").parent("tr");
        currentTr.parent().find("tr.tree-node-selected").removeClass("tree-node-selected");
        currentTr.addClass("tree-node-selected");

        var tabTemplate = $('#template-edit-tab-setting').html();
        var wrapper = $('#edit-tab-setting').empty().html(tabTemplate);

        ko.applyBindings(self, wrapper[0]);
        wrapper.find("table").tableDnD({ onDrop: self.tableDnDSort });
    };
    this.edit.resetTableKey = function () {
        var relationship = self.edit.selectedTab.relationship();
        self.data.tablekey([]);
        var cols = self.data.column();
        for (var i in cols)
            if (cols[i].IsIdentity || cols[i].IsPrimaryKey)
                self.data.tablekey.push(cols[i].ColumnName);

        self.edit.selectedTab.relationship(relationship);
        self.edit.selectedTab.primaryKeys(self.data.tablekey());
    };
    this.edit.setDefaultForm = function () {
        //if (self.edit.selectedTab.subtable()==
        var arr = [
            { field: 'ApproveState', title: '审批状态', type: 'text', readonly: true },
            { field: 'ApproveRemark', title: '审批意见', type: 'text', readonly: true },
            { field: 'ApprovePerson', title: '审批人', type: 'text', readonly: true },
            { field: 'ApproveDate', title: '审批日期', type: 'datebox', readonly: true },
            { field: 'CreatePerson', title: '编制人', type: 'text', readonly: true },
            { field: 'CreateDate', title: '编制日期', type: 'datebox', readonly: true },
            { field: 'UpdatePerson', title: '修改人', type: 'text', readonly: true },
            { field: 'UpdateDate', title: '修改日期', type: 'datebox', readonly: true }
        ];

        var cols = self.data.column();
        var defaults = { field: '', title: '', hidden: false, sortable: true, align: 'left', width: 80, formatter: '', editor: 'text', type: '', readonly: true };
        for (var i in arr) {
            if (!$.grep(cols, function (item) { return item.ColumnName == arr[i].field; }).length)
                return;

            arr[i] = $.extend({}, defaults, arr[i]);
        }

        self.edit.selectedTab.columns(arr);

        var tree = self.edit.columntree2.$element();
        for (var i in arr) {
            var node = tree.tree('find', arr[i].field);
            if (node) tree.tree('check', node.target);
        }
    };

    this.initWizard = function () {
        var stepTemplate = $('#template-' + self.form.type);
        var wizard = $('#wizard').removeData('smartWizard').empty();
        if (!stepTemplate.length) return;
        wizard.html(stepTemplate.html());
        wizard.smartWizard({
            labelNext: '下一步',
            labelPrevious: '上一步',
            labelFinish: '生成',
            onFinish: self.generator
        });
        var resizeStep = function () {
            $(".step").height($(window).height() - 145)
                      .width($(window).width() - 205);
            $(".actionBar").width($(window).width() - 195);
            var index = wizard.smartWizard('currentStep');
            wizard.smartWizard('goToStep', index);
        };
        $(window).resize(resizeStep);
        resizeStep();
        ko.applyBindings(self, wizard[0]);
        wizard.find("table").tableDnD({ onDrop: self.tableDnDSort });

        for (var i in self.form) {
            if ($.isFunction(self.form[i]))
                if (self.form[i]() instanceof Array)
                    if (self.form[i].subscribe) 
                        self.form[i].subscribe(self.tableDnDUpdate);
        }
    };
    this.resetWizard = function () {
        var wizard = $("#wizard").smartWizard('goToStep', 1);
        for (var i = 1; i <= wizard.find(">ul>li").length; i++)
            wizard.smartWizard("disableStep", i);
    };

    this.tableDnDUpdate = function () {
        setTimeout('$("table").tableDnDUpdate()', 300);
    };
   
    this.tableDnDSort = function (table, row) {
        var name = $(table).find("tbody").attr("data-bind").replace('foreach:form.','');
        var array = self.form[name], i = 0;

        if (name == 'foreach:edit.selectedTab.columns')
            array = self.edit.selectedTab.columns;

        $("tr[id]", table).each(function () { array()[this.id].id = i++; });
        array.sort(function (left, right) { return left.id == right.id ? 0 : (left.id < right.id ? -1 : 1) });

        //for fix ko bug refresh ui
        var tempArr = array();
        array([]);
        array(tempArr);
    };
};
 
