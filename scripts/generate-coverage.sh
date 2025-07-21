#!/bin/bash

# 单元测试覆盖率生成脚本
# 用法: ./scripts/generate-coverage.sh [options]
# 选项:
#   --threshold <number>    覆盖率阈值 (默认: 90)
#   --no-threshold-check    跳过覆盖率阈值检查
#   --output-dir <path>     覆盖率报告输出目录 (默认: CoverageReport)
#   --test-results <path>   测试结果目录 (默认: TestResults)
#   --help                  显示帮助信息

set -e

# 默认配置
COVERAGE_THRESHOLD=90
OUTPUT_DIR="CoverageReport"
TEST_RESULTS_DIR="TestResults"
CHECK_THRESHOLD=true
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
PROJECT_ROOT="$(dirname "$SCRIPT_DIR")"

# 颜色定义
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# 帮助信息
show_help() {
    echo "单元测试覆盖率生成脚本"
    echo ""
    echo "用法: $0 [options]"
    echo ""
    echo "选项:"
    echo "  --threshold <number>    覆盖率阈值 (默认: 90)"
    echo "  --no-threshold-check    跳过覆盖率阈值检查"
    echo "  --output-dir <path>     覆盖率报告输出目录 (默认: CoverageReport)"
    echo "  --test-results <path>   测试结果目录 (默认: TestResults)"
    echo "  --help                  显示帮助信息"
    echo ""
    echo "示例:"
    echo "  $0                                    # 使用默认设置"
    echo "  $0 --threshold 80                    # 设置覆盖率阈值为80%"
    echo "  $0 --no-threshold-check              # 跳过阈值检查"
    echo "  $0 --output-dir MyReport             # 自定义输出目录"
}

# 解析命令行参数
while [[ $# -gt 0 ]]; do
    case $1 in
        --threshold)
            COVERAGE_THRESHOLD="$2"
            shift 2
            ;;
        --no-threshold-check)
            CHECK_THRESHOLD=false
            shift
            ;;
        --output-dir)
            OUTPUT_DIR="$2"
            shift 2
            ;;
        --test-results)
            TEST_RESULTS_DIR="$2"
            shift 2
            ;;
        --help)
            show_help
            exit 0
            ;;
        *)
            echo -e "${RED}❌ 未知参数: $1${NC}"
            show_help
            exit 1
            ;;
    esac
done

# 日志函数
log_info() {
    echo -e "${BLUE}ℹ️  $1${NC}"
}

log_success() {
    echo -e "${GREEN}✅ $1${NC}"
}

log_warning() {
    echo -e "${YELLOW}⚠️  $1${NC}"
}

log_error() {
    echo -e "${RED}❌ $1${NC}"
}

# 检查工具是否安装
check_tool() {
    if ! command -v "$1" &> /dev/null; then
        log_error "$1 未安装，正在尝试安装..."
        return 1
    fi
    return 0
}

# 安装 ReportGenerator
install_reportgenerator() {
    log_info "安装 ReportGenerator..."
    if ! dotnet tool list -g | grep -q "dotnet-reportgenerator-globaltool"; then
        dotnet tool install -g dotnet-reportgenerator-globaltool
        log_success "ReportGenerator 安装完成"
    else
        log_info "ReportGenerator 已安装"
    fi
}

# 清理旧文件
cleanup() {
    log_info "清理旧的测试结果和覆盖率报告..."
    rm -rf "$PROJECT_ROOT/$TEST_RESULTS_DIR"
    rm -rf "$PROJECT_ROOT/$OUTPUT_DIR"
}

# 运行单元测试并生成覆盖率数据
run_tests() {
    log_info "运行单元测试并生成覆盖率数据..."

    cd "$PROJECT_ROOT"

    dotnet test \
        --configuration Release \
        --verbosity normal \
        --collect:"XPlat Code Coverage" \
        --results-directory "./$TEST_RESULTS_DIR/" \
        --logger trx \
        --logger "console;verbosity=detailed" \
        -- DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.Format=cobertura

    if [ $? -ne 0 ]; then
        log_error "单元测试失败!"
        exit 1
    fi

    log_success "单元测试完成!"

    # 列出生成的覆盖率文件
    log_info "生成的覆盖率文件:"
    find "./$TEST_RESULTS_DIR" -name "*.xml" -o -name "coverage.*" | head -10
}

# 查找覆盖率文件
find_coverage_files() {
    local coverage_files=""

    # 优先查找 cobertura 格式文件
    if find "./$TEST_RESULTS_DIR" -name "*.cobertura.xml" | grep -q .; then
        coverage_files=$(find "./$TEST_RESULTS_DIR" -name "*.cobertura.xml" | tr '\n' ';' | sed 's/;$//')
        log_info "找到 cobertura 格式覆盖率文件" >&2
    # 备选 opencover 格式文件
    elif find "./$TEST_RESULTS_DIR" -name "*.opencover.xml" | grep -q .; then
        coverage_files=$(find "./$TEST_RESULTS_DIR" -name "*.opencover.xml" | tr '\n' ';' | sed 's/;$//')
        log_info "找到 opencover 格式覆盖率文件" >&2
    # 查找通用 coverage.xml 文件
    elif find "./$TEST_RESULTS_DIR" -name "coverage.xml" | grep -q .; then
        coverage_files=$(find "./$TEST_RESULTS_DIR" -name "coverage.xml" | tr '\n' ';' | sed 's/;$//')
        log_info "找到通用 coverage.xml 文件" >&2
    else
        log_error "未找到覆盖率文件!" >&2
        exit 1
    fi

    echo "$coverage_files"
}

# 生成覆盖率报告
generate_report() {
    log_info "生成覆盖率报告..."

    local coverage_files=$(find_coverage_files)

    log_info "使用覆盖率文件模式: $coverage_files"

    reportgenerator \
        -reports:"$coverage_files" \
        -targetdir:"$OUTPUT_DIR" \
        -reporttypes:"Html;Cobertura;JsonSummary;TextSummary;Badges" \
        -verbosity:Info

    log_success "覆盖率报告生成完成!"
}

# 显示覆盖率详情
show_coverage_details() {
    log_info "覆盖率详细分析:"
    echo "=============================="

    # 显示整体摘要
    if [ -f "$OUTPUT_DIR/Summary.txt" ]; then
        echo ""
        log_info "整体覆盖率摘要:"
        cat "$OUTPUT_DIR/Summary.txt"
        echo ""
    fi

    # 解析并显示类级别覆盖率
    if [ -f "$OUTPUT_DIR/Summary.json" ]; then
        echo ""
        log_info "类级别覆盖率详情:"
        echo "--------------------------------"

        if command -v jq &> /dev/null; then
            # 使用 jq 解析 JSON
            jq -r '
                .coverage.assemblies[] |
                select(.name | contains("Ray.")) |
                .classesinassembly[] |
                select(.name != "") |
                "Class: \(.name)" +
                " | Line Coverage: \(.coverage)%" +
                " | Branch Coverage: \(.branchcoverage // "N/A")%" +
                " | Lines: \(.coveredlines)/\(.coverablelines)"
            ' "$OUTPUT_DIR/Summary.json" | sort -k3 -n

            echo ""
            log_warning "低覆盖率类 (< 50%):"
            echo "------------------------------------"

            jq -r '
                .coverage.assemblies[] |
                select(.name | contains("Ray.")) |
                .classesinassembly[] |
                select(.name != "" and (.coverage | type == "number") and (.coverage < 50)) |
                "❌ \(.name): \(.coverage)% (\(.coveredlines)/\(.coverablelines) lines)"
            ' "$OUTPUT_DIR/Summary.json" | sort -k2 -n

            echo ""
            log_success "高覆盖率类 (>= 80%):"
            echo "--------------------------------------"

            jq -r '
                .coverage.assemblies[] |
                select(.name | contains("Ray.")) |
                .classesinassembly[] |
                select(.name != "" and (.coverage | type == "number") and (.coverage >= 80)) |
                "✅ \(.name): \(.coverage)% (\(.coveredlines)/\(.coverablelines) lines)"
            ' "$OUTPUT_DIR/Summary.json" | sort -k2 -nr

        else
            log_warning "jq 未安装，无法显示详细的类级别覆盖率信息"
            log_info "请安装 jq 以获得更详细的覆盖率分析: sudo apt-get install jq"
        fi
    fi
}

# 检查覆盖率阈值
check_coverage_threshold() {
    if [ "$CHECK_THRESHOLD" = false ]; then
        log_info "跳过覆盖率阈值检查"
        return 0
    fi

    log_info "检查覆盖率阈值..."

    if [ -f "$OUTPUT_DIR/Summary.json" ]; then
        if command -v jq &> /dev/null; then
            local coverage=$(jq -r '.summary.linecoverage' "$OUTPUT_DIR/Summary.json")
            log_info "当前覆盖率: ${coverage}%"

            if (( $(echo "$coverage >= $COVERAGE_THRESHOLD" | bc -l) )); then
                log_success "覆盖率检查通过! (${coverage}% >= ${COVERAGE_THRESHOLD}%)"
                return 0
            else
                log_error "覆盖率检查失败! 当前: ${coverage}%, 要求: ${COVERAGE_THRESHOLD}%"
                return 1
            fi
        else
            log_warning "jq 未安装，无法检查覆盖率阈值"
            return 0
        fi
    else
        log_error "覆盖率报告文件未找到!"
        return 1
    fi
}

# 主函数
main() {
    echo "========================================"
    log_info "开始生成单元测试覆盖率报告"
    echo "========================================"
    echo ""
    log_info "配置信息:"
    echo "  项目根目录: $PROJECT_ROOT"
    echo "  测试结果目录: $TEST_RESULTS_DIR"
    echo "  覆盖率报告目录: $OUTPUT_DIR"
    echo "  覆盖率阈值: $COVERAGE_THRESHOLD%"
    echo "  检查阈值: $CHECK_THRESHOLD"
    echo ""

    # 检查必要工具
    if ! check_tool "dotnet"; then
        log_error ".NET SDK 未安装，请先安装 .NET SDK"
        exit 1
    fi

    # 安装 ReportGenerator
    install_reportgenerator

    # 清理旧文件
    cleanup

    # 运行测试
    run_tests

    # 生成报告
    generate_report

    # 显示详情
    show_coverage_details

    # 检查阈值
    local threshold_result=0
    check_coverage_threshold || threshold_result=$?

    echo ""
    echo "========================================"
    if [ $threshold_result -eq 0 ]; then
        log_success "覆盖率报告生成完成!"
        log_info "报告位置: $PROJECT_ROOT/$OUTPUT_DIR/index.html"
    else
        log_error "覆盖率报告生成完成，但未达到阈值要求!"
        log_info "报告位置: $PROJECT_ROOT/$OUTPUT_DIR/index.html"
        exit 1
    fi
    echo "========================================"
}

# 运行主函数
main "$@"
